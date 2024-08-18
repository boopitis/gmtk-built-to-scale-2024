using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * https://youtu.be/gIjajeyjRfE?feature=shared
 */
public class BeatInterval
{
    public event EventHandler OnTrigger;

    private readonly float subdivision;
    
    private int lastInterval;

    public BeatInterval(float subdivision)
    {
        this.subdivision = subdivision;
    }

    public float GetIntervalLength(float bpm)
    {
        const float seconds = 60f;
        return seconds / (bpm * subdivision);
    }

    public void CheckForNewInterval(float interval)
    {
        var intervalInt = Mathf.FloorToInt(interval);
        if (intervalInt == lastInterval) return;
        
        lastInterval = intervalInt;
        OnTrigger?.Invoke(this, EventArgs.Empty);
    }
}
