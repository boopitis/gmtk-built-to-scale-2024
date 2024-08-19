using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * https://youtu.be/gIjajeyjRfE?feature=shared
 */
public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance { get; private set; }
    
    public event EventHandler<OnCurrentSubdivisionChangeEventArgs> OnCurrentSubdivisionChange;
    public class OnCurrentSubdivisionChangeEventArgs : EventArgs
    {
        public int CurrentSubdivision;
    }
    
    [SerializeField] private float bpm;
    [SerializeField] private AudioSource audioSource;
    
    private MeasureInterval eighthMeasureInterval;
    private MeasureInterval fullMeasureInterval;
    
    private float timeSinceLastHalfMeasure;
    // Range from 0-15. Stores the current beat that the subdivision is on. (eg. 0 is beat 1, 15 is beat 4+)
    private int currentSubdivision, pCurrentSubdivision;

    private void Awake()
    {
        Instance = this;

        timeSinceLastHalfMeasure = 0f;
        currentSubdivision = 0;
        
        eighthMeasureInterval = new MeasureInterval(2); // Set to trigger every eighth note
        fullMeasureInterval = new MeasureInterval(1.0f/8); // Set to trigger every measure

        eighthMeasureInterval.OnTrigger += EighthMeasureIntervalOnTrigger;
        fullMeasureInterval.OnTrigger += FullMeasureIntervalOnTrigger;
    }

    private void FullMeasureIntervalOnTrigger(object sender, EventArgs e)
    {
        timeSinceLastHalfMeasure = 0f;
        currentSubdivision = 0;
        OnCurrentSubdivisionChange?.Invoke(this, new OnCurrentSubdivisionChangeEventArgs
        {
            CurrentSubdivision = currentSubdivision
        });
        pCurrentSubdivision = currentSubdivision;
    }

    private void EighthMeasureIntervalOnTrigger(object sender, EventArgs e)
    {
        if (pCurrentSubdivision == 15) return;
        currentSubdivision++;
        OnCurrentSubdivisionChange?.Invoke(this, new OnCurrentSubdivisionChangeEventArgs
        {
            CurrentSubdivision = currentSubdivision
        });
        pCurrentSubdivision = currentSubdivision;
    }

    private float debugTimer = 0f;
    private void Update()
    {
        timeSinceLastHalfMeasure += Time.deltaTime;

        const int beats = 2;
        if (SecondsToBeats(timeSinceLastHalfMeasure) > beats) timeSinceLastHalfMeasure = 0f;
        Debug.Log($"timeSinceLastBeat: {timeSinceLastHalfMeasure}");
        
        UpdateBeatInterval(eighthMeasureInterval);
        UpdateBeatInterval(fullMeasureInterval);
    }

    private void UpdateBeatInterval(MeasureInterval measureInterval)
    {
        float sampledTime = audioSource.timeSamples /
                            (audioSource.clip.frequency * measureInterval.GetIntervalLength(bpm));
        measureInterval.CheckForNewInterval(sampledTime);
    }

    // TODO Each beat takes 2/3 seconds
    private float SecondsToBeats(float seconds)
    {
        float secondsInMinute = 60f;
        return seconds * (bpm / secondsInMinute);
    }
}
