using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * https://youtu.be/gIjajeyjRfE?feature=shared
 */
public class MusicSyncManager : MonoBehaviour
{
    public static MusicSyncManager Instance { get; private set; }
    
    public event EventHandler<OnCurrentSubdivisionChangeEventArgs> OnCurrentSubdivisionChange;
    public class OnCurrentSubdivisionChangeEventArgs : EventArgs
    {
        public int CurrentSubdivision;
    }
    
    [SerializeField] private float bpm;
    [SerializeField] private AudioSource audioSource;
    
    private float halfMeasureLength;
    
    private MeasureInterval eighthMeasureInterval;
    private MeasureInterval twoMeasureInterval;
    
    private float timeToNextHalfMeasure;
    // Range from 0-15. Stores the current beat that the subdivision is on. (eg. 0 is beat 1, 15 is beat 4+)
    private int currentSubdivision, pCurrentSubdivision;
    
    private void Awake()
    {
        Instance = this;

        halfMeasureLength = BeatsToSeconds(2);
   
        eighthMeasureInterval = new MeasureInterval(2); // Set to trigger every eighth note
        twoMeasureInterval = new MeasureInterval(1.0f/8); // Set to trigger every 2 measures
        
        timeToNextHalfMeasure = halfMeasureLength;
        currentSubdivision = 0;

        eighthMeasureInterval.OnTrigger += EighthMeasureInterval_OnTrigger;
        twoMeasureInterval.OnTrigger += TwoMeasureInterval_OnTrigger;
    }

    private void TwoMeasureInterval_OnTrigger(object sender, EventArgs e)
    {
        timeToNextHalfMeasure = halfMeasureLength;
        currentSubdivision = 0;
        OnCurrentSubdivisionChange?.Invoke(this, new OnCurrentSubdivisionChangeEventArgs
        {
            CurrentSubdivision = currentSubdivision
        });
        pCurrentSubdivision = currentSubdivision;
    }

    private void EighthMeasureInterval_OnTrigger(object sender, EventArgs e)
    {
        if (pCurrentSubdivision == 15) return;
        currentSubdivision++;
        OnCurrentSubdivisionChange?.Invoke(this, new OnCurrentSubdivisionChangeEventArgs
        {
            CurrentSubdivision = currentSubdivision
        });
        pCurrentSubdivision = currentSubdivision;
    }

    private void Update()
    {
        timeToNextHalfMeasure -= Time.deltaTime;
        
        if (SecondsToBeats(timeToNextHalfMeasure) <= 0) timeToNextHalfMeasure = halfMeasureLength;
        
        UpdateBeatInterval(eighthMeasureInterval);
        UpdateBeatInterval(twoMeasureInterval);
    }

    private void UpdateBeatInterval(MeasureInterval measureInterval)
    {
        float sampledTime = audioSource.timeSamples /
                            (audioSource.clip.frequency * measureInterval.GetIntervalLength(bpm));
        measureInterval.CheckForNewInterval(sampledTime);
    }

    private float SecondsToBeats(float seconds)
    {
        const float secondsInMinute = 60f;
        return seconds * (bpm / secondsInMinute);
    }

    private float BeatsToSeconds(float beats)
    {
        const float secondsInMinute = 60f;
        return beats * (secondsInMinute / bpm);
    }

    public float GetTimeToLastHalfMeasure() => halfMeasureLength - timeToNextHalfMeasure;

    public float GetTimeToNextHalfMeasure() => timeToNextHalfMeasure;

    public int GetLastHalfMeasureSubdivision()
    { 
        return currentSubdivision / 4 * 4;
    }

    public int GetNextHalfMeasureSubdivision()
    {
        return (currentSubdivision / 4 * 4 + 4) % 16;
    }
}
