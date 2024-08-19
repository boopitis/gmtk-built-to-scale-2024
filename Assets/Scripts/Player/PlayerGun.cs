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
    private int noteIndex;
    // Order which subdivisions are played in
    private static int[] _subdivisionTimingOrder = { 0, 12, 8, 4, 10, 6, 2, 11, 9, 7, 5, 3, 1 };
    // Which subdivisions get played
    private List<int> subdivisionTiming;

    private void Awake()
    {
        Instance = this;
        
        noteIndex = 0;
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
        int accuracyInMillis;
        int subdivision;
        
        float timeToLastHalfMeasure = MusicSyncManager.Instance.GetTimeToLastHalfMeasure();
        float timeToNextHalfMeasure = MusicSyncManager.Instance.GetTimeToNextHalfMeasure();

        if (timeToLastHalfMeasure < timeToNextHalfMeasure)
        {
            accuracyInMillis = (int)(timeToLastHalfMeasure * 1000);
            subdivision = MusicSyncManager.Instance.GetLastHalfMeasureSubdivision();
        } else
        {
            accuracyInMillis = (int)(timeToNextHalfMeasure * 1000);
            subdivision = MusicSyncManager.Instance.GetNextHalfMeasureSubdivision();
        }

        if (accuracyInMillis < timingWindowInMillis)
        {
            QueueNotes(subdivision);
        }
    }

    private void QueueNotes(int subdivision)
    {
        if (subdivision % 4 != 0)
        {
            Debug.LogError("Subdivision does not correspond with a half measure!");
        }
    }

    private void PlayerMusicScaleManager_OnCurrentNotesChanged(object sender, EventArgs e)
    {
        SetSubdivisionTiming();
    }

    private void SetSubdivisionTiming()
    {
        var currentNoteSOListSize = PlayerMusicScaleManager.Instance.GetCurrentNoteSOList().Count;
        subdivisionTiming = new List<int>(_subdivisionTimingOrder[..currentNoteSOListSize]);
        
        subdivisionTiming.Sort();
    }

    private void MusicSyncManager_OnCurrentSubdivisionChange(object sender, MusicSyncManager.OnCurrentSubdivisionChangeEventArgs e)
    {
        if (subdivisionTiming[noteIndex] != e.CurrentSubdivision) return;
        
        Attack();
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

    public void Attack()
    {
        var firedNoteSO = PlayerMusicScaleManager.Instance.GetCurrentNoteSOList()[noteIndex];

        OnAttack?.Invoke(this, new OnAttackEventArgs
        {
            FiredNoteSO = firedNoteSO
        });
        
        do // do/while exists so break; is usable
        {
            if (noteIndex != PlayerMusicScaleManager.Instance.GetCurrentNoteSOList().Count - 1 ||
                PlayerMusicScaleManager.Instance.GetCreatedScaleSO() is null)
            { // Fire normal projectile
                Projectile.SpawnProjectile(firedNoteSO.prefab, firePointTransform, transform.rotation, out _);
                break;
            }
            
            // Fire special
            PlayerMusicScaleManager.Instance.GetCreatedScaleSO().special.Fire(firePointTransform, transform.rotation);
        } while (false);
        
        noteIndex++;
        if (noteIndex == PlayerMusicScaleManager.Instance.GetCurrentNoteSOList().Count) noteIndex = 0;
    }
}