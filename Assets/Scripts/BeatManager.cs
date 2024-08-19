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

    private BeatInterval beatInterval;
    // Range from 0-15. Stores the current beat that the subdivision is on. (eg. 0 is beat 1, 15 is beat 4+)
    private int currentSubdivision;

    private void Awake()
    {
        Instance = this;
        
        currentSubdivision = 0;
        
        beatInterval = new BeatInterval(2); // Set to trigger every eighth note
        beatInterval.OnTrigger += BeatInterval_OnTrigger;
    }

    private void BeatInterval_OnTrigger(object sender, EventArgs e)
    {
        currentSubdivision = (currentSubdivision + 1) % 16;
        OnCurrentSubdivisionChange?.Invoke(this, new OnCurrentSubdivisionChangeEventArgs
        {
            CurrentSubdivision = currentSubdivision
        });
    }

    private void Update()
    {
        float sampledTime = audioSource.timeSamples /
                            (audioSource.clip.frequency * beatInterval.GetIntervalLength(bpm));
        beatInterval.CheckForNewInterval(sampledTime);
    }
}
