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

    private BeatInterval eighthBeatInterval;
    private BeatInterval measureBeatInterval;
    // Range from 0-15. Stores the current beat that the subdivision is on. (eg. 0 is beat 1, 15 is beat 4+)
    private int currentSubdivision, pCurrentSubdivision;

    private void Awake()
    {
        Instance = this;
        
        currentSubdivision = 0;
        
        eighthBeatInterval = new BeatInterval(2); // Set to trigger every eighth note
        measureBeatInterval = new BeatInterval(1.0f/8); // Set to trigger every measure

        eighthBeatInterval.OnTrigger += EighthBeatInterval_OnTrigger;
        measureBeatInterval.OnTrigger += MeasureBeatInterval_OnTrigger;
    }

    private void MeasureBeatInterval_OnTrigger(object sender, EventArgs e)
    {
        currentSubdivision = 0;
        OnCurrentSubdivisionChange?.Invoke(this, new OnCurrentSubdivisionChangeEventArgs
        {
            CurrentSubdivision = currentSubdivision
        });
        pCurrentSubdivision = currentSubdivision;
    }

    private void EighthBeatInterval_OnTrigger(object sender, EventArgs e)
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
        UpdateBeatInterval(eighthBeatInterval);
        UpdateBeatInterval(measureBeatInterval);
    }

    private void UpdateBeatInterval(BeatInterval beatInterval)
    {
        float sampledTime = audioSource.timeSamples /
                            (audioSource.clip.frequency * beatInterval.GetIntervalLength(bpm));
        beatInterval.CheckForNewInterval(sampledTime);
    }
}
