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
public class PlayerMusicScaleManager : MonoBehaviour
{
    public static PlayerMusicScaleManager Instance { get; private set; }

    public event EventHandler<OnScaleCreatedEventArgs> OnScaleChanged;
    public class OnScaleCreatedEventArgs : EventArgs
    {
        public ScaleSO ScaleSO;
    }

    public event EventHandler OnCurrentNotesChanged;

    [SerializeField] private ScaleListSO scaleListSO;

    [SerializeField] private List<NoteSO> currentNoteSOList; // TODO REMOVE SERIALIZEFIELD DEBUG
    [FormerlySerializedAs("createdScaleSO")][SerializeField] private ScaleSO currentScaleSO; // TODO REMOVE SERIALIZEFIELD DEBUG
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
        if (potentialNoteSO.pitch is 0 or 12)
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
        if (potentialNoteSO.pitch is 0 or 12)
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
        ScaleSO newCreatedScaleSO = null;

        foreach (var scaleSO in scaleListSO.scaleSOs)
        {
            if (scaleSO.noteSOList.Length != currentNoteSOList.Count) continue;

            var validScale = !scaleSO.noteSOList.Where((noteSO, j) =>
                noteSO.pitch != currentNoteSOList[j].pitch).Any();

            if (!validScale) continue;

            newCreatedScaleSO = scaleSO;

            break;
        }

        if (newCreatedScaleSO == currentScaleSO) return;
        if (newCreatedScaleSO == null) return;

        currentScaleSO = newCreatedScaleSO;
        OnScaleChanged?.Invoke(this, new OnScaleCreatedEventArgs
        {
            ScaleSO = currentScaleSO
        });
    }

    public List<NoteSO> GetCurrentNoteSOList() => currentNoteSOList;

    public ScaleSO GetCreatedScaleSO() => currentScaleSO;
}
