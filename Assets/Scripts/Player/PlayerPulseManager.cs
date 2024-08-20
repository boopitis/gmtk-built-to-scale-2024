using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPulseManager : MonoBehaviour
{
    [SerializeField] private Transform visual;
    
    private const float MaxScale = 3.1f;

    private float currentRatio;

    private void Awake()
    {
        currentRatio = MaxScale;
    }

    private void Update()
    {
        MusicSyncManager.Instance.GetHalfMeasureSubdivisionLengthInBeats();
        var timeToNextHalfMeasure = MusicSyncManager.Instance.GetTimeToNextHalfMeasure();

        visual.localScale = Vector3.one * (MaxScale - (float)Math.Abs(MaxScale * Math.Sin(Math.PI *
            timeToNextHalfMeasure /
            (MusicSyncManager.Instance
                .GetHalfMeasureSubdivisionLengthInSeconds() / 2))));
    }
}
