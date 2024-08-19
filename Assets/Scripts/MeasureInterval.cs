using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * https://youtu.be/gIjajeyjRfE?feature=shared
 */
public class MeasureInterval
{
    public event EventHandler OnTrigger;

    private readonly float subdivision;
    
    private int pInterval;

    public MeasureInterval(float subdivision)
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
        if (intervalInt == pInterval) return;
        
        pInterval = intervalInt;
        OnTrigger?.Invoke(this, EventArgs.Empty);
    }
}
