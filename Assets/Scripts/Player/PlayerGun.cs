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

    [SerializeField] private SpriteRenderer weaponRenderer;

    [SerializeField] private Transform firePointTransform;
    [SerializeField] private Transform pivotTransform;

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
        MusicSyncManager.Instance.OnTwoMeasureIntervalTriggered += MusicSyncManager_OnTwoMeasureIntervalTriggered;

        SetSubdivisionTiming();
    }

    private void MusicSyncManager_OnTwoMeasureIntervalTriggered(object sender, EventArgs e)
    {
        pSubdivisionOnShoot = -1;
    }

    private void GameInput_OnPlayerShootPerformed(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsPaused()) return;
        
        if (!MusicSyncManager.Instance.GetFirstTwoMeasureIntervalTriggered()) return;

        int accuracyInMillis;
        int subdivision;

        float timeToLastHalfMeasure = MusicSyncManager.Instance.GetTimeToLastHalfMeasure();
        float timeToNextHalfMeasure = MusicSyncManager.Instance.GetTimeToNextHalfMeasure();

        if (timeToLastHalfMeasure < timeToNextHalfMeasure)
        {
            accuracyInMillis = (int)(timeToLastHalfMeasure * 1000);
            subdivision = MusicSyncManager.Instance.GetLastHalfMeasureSubdivision();
        }
        else
        {
            accuracyInMillis = (int)(timeToNextHalfMeasure * 1000);
            subdivision = MusicSyncManager.Instance.GetNextHalfMeasureSubdivision();
        }

        if (accuracyInMillis >= timingWindowInMillis) return;
        
        QueueNotes(subdivision);
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

        var index = subdivisionTiming.IndexOf(subdivision);

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
        if (GameManager.Instance.IsPaused()) return;
        
        pointerPositionInput = GameInput.Instance.GetPlayerPointerPositionVector2InWorldSpace();
        Vector2 direction = (pointerPositionInput - (Vector2)pivotTransform.position).normalized;
        pivotTransform.right = direction;
        transform.position = (Vector2)pivotTransform.position + (0.65f * direction);
        transform.rotation = pivotTransform.rotation;
    }

    private bool TryAttack(int currentSubdivision)
    {

        if (queuedNotes.Count == 0) return false;

        var queuedNote = queuedNotes[0];

        if (queuedNote.Subdivision != currentSubdivision) return false;

        OnAttack?.Invoke(this, new OnAttackEventArgs
        {
            FiredNoteSO = queuedNote.NoteSO
        });

        if (queuedNote.Subdivision == 12)
        {
            if (PlayerMusicScaleManager.Instance.GetCreatedScaleSO() is not null)
            {
                // Fire special
                StartCoroutine(DelaySpecialAttack());
                PlayerGunAnimations.Instance.SpecialAttackAnimation(PlayerMusicScaleManager.Instance.GetCreatedScaleSO().name);
                queuedNotes.RemoveAt(0);
                return true;
            }
        }

        // Fire normal projectile
        Projectile.SpawnProjectile(queuedNote.NoteSO.prefab, firePointTransform, transform.rotation, out _);
        PlayerGunAnimations.Instance.BasicAttackAnimation();
        queuedNotes.RemoveAt(0);
        return true;
    }

    private IEnumerator DelaySpecialAttack()
    {
        yield return new WaitForSeconds(PlayerMusicScaleManager.Instance.GetCreatedScaleSO().specialAnimationDelay);
        PlayerMusicScaleManager.Instance.GetCreatedScaleSO().special.Fire(firePointTransform, transform.rotation);
    }
}