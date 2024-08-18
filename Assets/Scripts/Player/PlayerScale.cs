using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    [SerializeField] private List<NoteSO> currentNoteSOList; // SERIALIZEFIELD DEBUG

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private NoteSO debug1;
    [SerializeField] private NoteSO debug2;
    [SerializeField] private NoteSO debug3;
    private void Update() //DEBUG
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TryAddNote(debug1);
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            TryRemoveNote(debug1);
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            TryAddNote(debug3);
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            TryAddNote(debug2);
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            TryRemoveNote(debug2);
        }
    }

    public bool TryAddNote(NoteSO potentialNoteSO)
    {
        if (potentialNoteSO.pitch == 0)
        {
            Debug.LogError("Cannot add a C note!");
        }
        
        for (var i = 0; i < currentNoteSOList.Count-1; i++)
        {
            var noteSO = currentNoteSOList[i];
            
            if (potentialNoteSO.pitch == noteSO.pitch) return false;

            if (potentialNoteSO.pitch > noteSO.pitch) continue;
            
            currentNoteSOList.Insert(i, potentialNoteSO);
            CheckScaleMatch();
            return true;
        }
        
        currentNoteSOList.Insert(currentNoteSOList.Count-1, potentialNoteSO);
        CheckScaleMatch();
        return true;
    }

    public bool TryRemoveNote(NoteSO potentialNoteSO)
    {
        if (potentialNoteSO.pitch == 0)
        {
            Debug.LogError("Cannot remove a C note!");
        }
        
        var removalSuccessful =  currentNoteSOList.Remove(potentialNoteSO);
        
        if (removalSuccessful) CheckScaleMatch();
        return removalSuccessful;
    }

    /**
     * Checks against all scales in scaleListSO to see if current notes match any scale in the scaleListSO.
     */
    private void CheckScaleMatch()
    {
        foreach (var scaleSO in scaleListSO.scaleSOs)
        {
            Debug.Log($"Testing {scaleSO.name}...");
            if (scaleSO.noteSOList.Length != currentNoteSOList.Count) continue;
                
            var validScale = !scaleSO.noteSOList.Where((noteSO, j) => 
                noteSO.pitch != currentNoteSOList[j].pitch).Any();

            if (!validScale) continue;
            
            OnScaleCreated?.Invoke(this, new OnScaleCreatedEventArgs
            {
                ScaleSO = scaleSO
            });
            Debug.Log($"{scaleSO.name} created!"); //DEBUG
            break;
        }
    }
}
