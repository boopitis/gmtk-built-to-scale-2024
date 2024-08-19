using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * Manages the notes that the Player currently has.
 * When a scale is completed, OnScaleCreated is fired with the relevant scale.
 */
public class PlayerMusicScale : MonoBehaviour
{

    public static PlayerMusicScale Instance { get; private set; }

    public event EventHandler<OnScaleCreatedEventArgs> OnScaleCreated;
    public class OnScaleCreatedEventArgs : EventArgs
    {
        public ScaleSO ScaleSO;
    }

    public event EventHandler OnCurrentNotesChanged;

    [SerializeField] private ScaleListSO scaleListSO;
    [SerializeField] private int maxCreatedScales;

    [SerializeField] private List<NoteSO> currentNoteSOList; // TODO REMOVE SERIALIZEFIELD DEBUG
    [SerializeField] private List<ScaleSO> createdScaleSOList;

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

    /**
     * Attempts to add a note to currentNoteSOList.
     * Returns bool depending on if adding was successful.
     * Runs CheckScaleMatch() if successful.
     */
    public bool TryAddNote(NoteSO potentialNoteSO)
    {
        if (potentialNoteSO.pitch == 0)
        {
            Debug.LogError("Cannot add a C note!");
        }

        for (var i = 0; i < currentNoteSOList.Count - 1; i++)
        {
            var noteSO = currentNoteSOList[i];

            if (potentialNoteSO.pitch == noteSO.pitch) return false;

            if (potentialNoteSO.pitch > noteSO.pitch) continue;

            currentNoteSOList.Insert(i, potentialNoteSO);
            CheckScaleMatch();
            OnCurrentNotesChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        currentNoteSOList.Insert(currentNoteSOList.Count - 1, potentialNoteSO);
        CheckScaleMatch();
        OnCurrentNotesChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /**
     * Attempts to remove a note from currentNoteSOList.
     * Returns bool depending on if removal was successful.
     * Runs CheckScaleMatch() if successful.
     */
    public bool TryRemoveNote(NoteSO potentialNoteSO)
    {
        if (potentialNoteSO.pitch == 0)
        {
            Debug.LogError("Cannot remove a C note!");
        }

        var removalSuccessful = currentNoteSOList.Remove(potentialNoteSO);

        if (!removalSuccessful) return false;

        CheckScaleMatch();
        OnCurrentNotesChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /**
     * Checks against all scales in scaleListSO to see if current notes match any scale in the scaleListSO.
     */
    private void CheckScaleMatch()
    {
        foreach (var scaleSO in scaleListSO.scaleSOs)
        {
            createdScaleSOList.Clear();

            if (scaleSO.noteSOList.Count != currentNoteSOList.Count) continue;

            var validScale = !scaleSO.noteSOList.Where((noteSO, j) =>
                noteSO.pitch != currentNoteSOList[j].pitch).Any();

            if (!validScale)
                continue;
            OnScaleCreated?.Invoke(this, new OnScaleCreatedEventArgs
            {
                ScaleSO = scaleSO
            });

            // var alreadyCreated = createdScaleSOList.Any(createdScaleSO => 
            //     scaleSO.scaleName == createdScaleSO.scaleName);

            // if (alreadyCreated) return;

            createdScaleSOList.Add(scaleSO);

            for (int i = 0; i < scaleListSO.scaleSOs.Length; i++)
            {
                if (scaleSO == scaleListSO.scaleSOs[i])
                {
                    ScaleViewer.Instance.CreateScalePanel(i);
                }
            }

            // if (createdScaleSOList.Count <= maxCreatedScales) return;

            // createdScaleSOList.RemoveAt(0);

            return;
        }
    }

    public List<NoteSO> GetCurrentNoteSOList() => currentNoteSOList;
    public void SetCurrentNoteSOList(List<NoteSO> newNoteSOList)
    {
        currentNoteSOList = newNoteSOList;
        CheckScaleMatch();
    }

    public List<ScaleSO> GetScaleSpecialsNeedingFiring(int index)
    {
        return createdScaleSOList.Where(scaleSO =>
            scaleSO.special.IsNeedingFiring(currentNoteSOList.Count, index)).ToList();
    }
}
