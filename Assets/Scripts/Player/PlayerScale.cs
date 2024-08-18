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
            if (scaleSO.noteSOList.Length != noteSOList.Count) continue;
                
            var validScale = true;

            for (var j = 0; j < scaleSO.noteSOList.Length; j++)
            {
                var noteSO = scaleSO.noteSOList[j];
                if (noteSO == noteSOList[j]) continue;

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
