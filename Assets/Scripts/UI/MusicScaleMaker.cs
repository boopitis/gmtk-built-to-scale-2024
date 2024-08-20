using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class MusicScaleMaker : MonoBehaviour
{
    public static MusicScaleMaker Instance { get; private set; }


    [SerializeField] private GameObject[] keyImages;
    [SerializeField] private NoteSO[] noteSOS;
    [SerializeField] private int allowedChanges = 3;
    private int changes;

    private List<int> storedChanges;

    private void Start()
    {
        Instance = this;

        ResetChanges();
    }

    public void ToggleKey(int key)
    {
        storedChanges.Add(key);
        if (changes > 0)
        {
            if (keyImages[key].activeInHierarchy)
            {
                keyImages[key].SetActive(false);
                PlayerMusicScaleManager.Instance.TryRemoveNote(noteSOS[key]);
            }
            else
            {
                keyImages[key].SetActive(true);
                PlayerMusicScaleManager.Instance.TryAddNote(noteSOS[key]);
            }
            print(storedChanges.Last());

            changes--;
        }
    }

    public void Undo()
    {
        if (storedChanges == null) return;

        int key = storedChanges.Last();
        storedChanges.RemoveAt(storedChanges.Count - 1);
        print(storedChanges.Last());

        if (keyImages[key].activeInHierarchy)
        {
            keyImages[key].SetActive(false);
            PlayerMusicScaleManager.Instance.TryRemoveNote(noteSOS[key]);
        }
        else
        {
            keyImages[key].SetActive(true);
            PlayerMusicScaleManager.Instance.TryAddNote(noteSOS[key]);
        }

        changes++;
    }

    public void ResetChanges()
    {
        changes = allowedChanges;
    }

    public void ResetStoredChanges()
    {
        storedChanges = null;
    }
}
