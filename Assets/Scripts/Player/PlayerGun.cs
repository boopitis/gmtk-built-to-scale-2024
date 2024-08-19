using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerGun : MonoBehaviour
{
    public static PlayerGun Instance { get; private set; }

    public event EventHandler<OnAttackEventArgs> OnAttack;
    public class OnAttackEventArgs : EventArgs
    {
        public NoteSO FiredNoteSO;
    }
    
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private SpriteRenderer weaponRenderer;
    
    [SerializeField] private Transform firePointTransform;

    [SerializeField] private int timingWindowInMillis;

    private Vector2 pointerPositionInput;
    // Order which subdivisions are played in
    private static readonly int[] SubdivisionTimingOrder = { 0, 12, 8, 4, 10, 6, 2, 11, 9, 7, 5, 3, 1 };
    // Which subdivisions get played
    private List<int> subdivisionTiming;

    private struct QueuedNote
    {
        public NoteSO NoteSO;
        public int Subdivision;
        public QueuedNote(NoteSO noteSO, int subdivision)
        {
            NoteSO = noteSO;
            Subdivision = subdivision;
        }
    }
    private List<QueuedNote> queuedNotes;
    private int pSubdivisionOnShoot;

    private void Awake()
    {
        Instance = this;
        
        queuedNotes = new List<QueuedNote>();
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerShootPerformed += GameInput_OnPlayerShootPerformed;
        
        PlayerMusicScaleManager.Instance.OnCurrentNotesChanged += PlayerMusicScaleManager_OnCurrentNotesChanged;
        MusicSyncManager.Instance.OnCurrentSubdivisionChange += MusicSyncManager_OnCurrentSubdivisionChange;
        
        SetSubdivisionTiming();
    }

    private void GameInput_OnPlayerShootPerformed(object sender, EventArgs e)
    {
        if (!MusicSyncManager.Instance.GetFirstTwoMeasureIntervalTriggered()) return;
        string debugChecker;
        
        int accuracyInMillis;
        int subdivision;
        
        float timeToLastHalfMeasure = MusicSyncManager.Instance.GetTimeToLastHalfMeasure();
        float timeToNextHalfMeasure = MusicSyncManager.Instance.GetTimeToNextHalfMeasure();

        if (timeToLastHalfMeasure < timeToNextHalfMeasure)
        {
            accuracyInMillis = (int)(timeToLastHalfMeasure * 1000);
            subdivision = MusicSyncManager.Instance.GetLastHalfMeasureSubdivision();
            debugChecker = "last";
        } else
        {
            accuracyInMillis = (int)(timeToNextHalfMeasure * 1000);
            subdivision = MusicSyncManager.Instance.GetNextHalfMeasureSubdivision();
            debugChecker = "next";
        }

        if (accuracyInMillis < timingWindowInMillis)
        {
            Debug.Log($"queueing on subdivision {subdivision} for {debugChecker}");
            QueueNotes(subdivision);
        }
    }

    private void PlayerMusicScaleManager_OnCurrentNotesChanged(object sender, EventArgs e)
    {
        SetSubdivisionTiming();
    }
    
    private void MusicSyncManager_OnCurrentSubdivisionChange(object sender, MusicSyncManager.OnCurrentSubdivisionChangeEventArgs e)
    {
        TryAttack(e.CurrentSubdivision);
    }
    
    private void QueueNotes(int subdivision)
    {
        if (subdivision % MusicSyncManager.Instance.GetHalfMeasureSubdivisionLength() != 0)
        {
            Debug.LogError("Subdivision does not correspond with a half measure!");
        }

        if (subdivision == pSubdivisionOnShoot) return;

        int index = subdivisionTiming.IndexOf(subdivision);

        if (index == -1) return;

        do
        {
            var queuedNote = new QueuedNote(
                PlayerMusicScaleManager.Instance.GetCurrentNoteSOList()[index],
                subdivisionTiming[index]);
            
            queuedNotes.Add(queuedNote);
            index++;
            
            if (index == subdivisionTiming.Count) break;
        } while (subdivisionTiming[index] < 
                 subdivision + MusicSyncManager.Instance.GetHalfMeasureSubdivisionLength());

        TryAttack(subdivision);

        pSubdivisionOnShoot = subdivision;
    }

    private void SetSubdivisionTiming()
    {
        var currentNoteSOListSize = PlayerMusicScaleManager.Instance.GetCurrentNoteSOList().Count;
        subdivisionTiming = new List<int>(SubdivisionTimingOrder[..currentNoteSOListSize]);
        
        subdivisionTiming.Sort();
    }

    private void Update()
    {
        UpdateFacingDirection();
    }

    private void UpdateFacingDirection()
    {
        pointerPositionInput = GameInput.Instance.GetPlayerPointerPositionVector2InWorldSpace();
        Vector2 direction = (pointerPositionInput - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (PlayerVisual.Instance.GetLookDirection().x < 0)
        {
            scale.y = -1;
            scale.x = -1;
        }
        else if (PlayerVisual.Instance.GetLookDirection().x > 0)
        {
            scale.y = 1;
            scale.x = 1;
        }
        transform.localScale = scale;

        if (transform.eulerAngles.z is > 0 and < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }

    private bool TryAttack(int currentSubdivision)
    {
        string debugPrint = queuedNotes.Aggregate("queuedNotes: ", (current, queuedNote) => 
            current + (queuedNote.Subdivision + ", "));
        debugPrint += $"; on subdivision {currentSubdivision}";
        Debug.Log(debugPrint);
        
        if (queuedNotes.Count == 0) return false;
        
        var queuedNote = queuedNotes[0];

        if (queuedNote.Subdivision != currentSubdivision) return false;

        OnAttack?.Invoke(this, new OnAttackEventArgs
        {
            FiredNoteSO = queuedNote.NoteSO
        });
        Debug.Log($"subdivision {queuedNote.Subdivision} fired");

        if (queuedNote.Subdivision == 12)
        {
            if (PlayerMusicScaleManager.Instance.GetCreatedScaleSO() is not null)
            {
                // Fire special
                PlayerMusicScaleManager.Instance.GetCreatedScaleSO().special.Fire(firePointTransform, transform.rotation);
                queuedNotes.RemoveAt(0);
                return true;
            }
        } 
        
        // Fire normal projectile
        Projectile.SpawnProjectile(queuedNote.NoteSO.prefab, firePointTransform, transform.rotation, out _);
        queuedNotes.RemoveAt(0);
        return true;
    }
}