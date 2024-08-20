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
        PlayerMusicScaleManager.Instance.OnCurrentNotesChanged += PlayerMusicScaleManager_OnCurrentNotesChanged;
        BeatManager.Instance.OnCurrentSubdivisionChange += BeatManager_OnCurrentSubdivisionChange;

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

    private void SetSubdivisionTiming()
    {
        var currentNoteSOListSize = PlayerMusicScaleManager.Instance.GetCurrentNoteSOList().Count;
        subdivisionTiming = new List<int>(_subdivisionTimingOrder[..currentNoteSOListSize]);

        subdivisionTiming.Sort();
    }

    private void BeatManager_OnCurrentSubdivisionChange(object sender, BeatManager.OnCurrentSubdivisionChangeEventArgs e)
    {
        //Debug.Log(e.CurrentSubdivision);
        if (subdivisionTiming[noteIndex] != e.CurrentSubdivision) return;

        //Debug.Log("fired projectile!");
        Attack();
    }

    private void Update()
    {
        UpdateFacingDirection();
    }

    private void UpdateFacingDirection()
    {
        pointerPositionInput = GameInput.Instance.GetPlayerPointerPositionVector2InWorldSpace();
        Vector2 direction = (pointerPositionInput - (Vector2)pivotTransform.position).normalized;
        pivotTransform.right = direction;
        transform.position = (Vector2)pivotTransform.position + (1.175f * direction);
        transform.rotation = pivotTransform.rotation;
    }

    public void Attack()
    {
        if (queuedNotes.Count == 0) return false;

        var queuedNote = queuedNotes[0];

        if (queuedNote.Subdivision != currentSubdivision) return false;

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
            Debug.Log(PlayerMusicScaleManager.Instance.GetCreatedScaleSO().name); //DEBUG
            PlayerMusicScaleManager.Instance.GetCreatedScaleSO().special.Fire(firePointTransform, transform.rotation);
        } while (false);

        noteIndex++;
        if (noteIndex == PlayerMusicScaleManager.Instance.GetCurrentNoteSOList().Count) noteIndex = 0;
    }
}