using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScale : MonoBehaviour
{

    public static PlayerScale Instance { get; private set; }

    public event EventHandler<OnScaleCreatedEventArgs> OnScaleCreated;
    public class OnScaleCreatedEventArgs : EventArgs
    {
        public ScaleSO ScaleSO;
    }

    [SerializeField] private ScaleListSO scaleListSO;
    
    private NoteProjectile[] noteList;

    private void Awake()
    {
        Instance = this;
    }

    // public void AddNote(Note note)
    // {
    //     
    // }

    private void CheckScaleMatch()
    {
        foreach (var scaleSO in scaleListSO.scaleSOs)
        {
            bool validScale = true;
            for (var j = 0; j < scaleSO.intervals.Length; j++)
            {
                int interval = scaleSO.intervals[j];
                if (noteList[j + 1].GetPitch() - noteList[j].GetPitch() == interval) continue;
                
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
