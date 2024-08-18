using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Manages the notes that the Player currently has.
 * When a scale is completed, OnScaleCreated is fired with the relevant scale.
 */
public class PlayerScale : MonoBehaviour
{

    public static PlayerScale Instance { get; private set; }

    public event EventHandler<OnScaleCreatedEventArgs> OnScaleCreated;
    public class OnScaleCreatedEventArgs : EventArgs
    {
        public ScaleSO ScaleSO;
    }

    [SerializeField] private ScaleListSO scaleListSO;
    
    private List<NoteSO> noteSOList;

    private void Awake()
    {
        Instance = this;

        noteSOList = new List<NoteSO>();
    }

    public void AddNote(int pitch)
    {
        
    }

    private void CheckScaleMatch()
    {
        foreach (var scaleSO in scaleListSO.scaleSOs)
        {
            if (scaleSO.intervals.Length+1 != noteSOList.Count) continue;
                
            var validScale = true;
         
            for (var j = 0; j < scaleSO.intervals.Length; j++)
            {
                int interval = scaleSO.intervals[j];
                if (noteSOList[j + 1].pitch - noteSOList[j].pitch == interval) continue;
                
                validScale = false;
                break;
            }
            
            if (!validScale) continue;
            
            OnScaleCreated?.Invoke(this, new OnScaleCreatedEventArgs
            {
                ScaleSO = scaleSO
            });
            Debug.Log($"{scaleSO.name} scale created!");
            break;
        }
    }
}
