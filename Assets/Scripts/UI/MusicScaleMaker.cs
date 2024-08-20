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

    private bool scaleMakerOpen = false;
    [SerializeField] private GameObject[] keyImages;
    [SerializeField] private NoteSO[] noteSOS;
    [SerializeField] private int allowedChanges = 3;
    private int changes;
    private List<int> storedChanges = new List<int>();

    private void Start()
    {
        Instance = this;

        GameManager.Instance.OnPause += OpenScaleMaker;
        GameManager.Instance.OnResume += CloseScaleMaker;

        ResetChanges();
    }

    public void ToggleKey(int key)
    {
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
            storedChanges.Add(key);
            changes--;
        }
    }

    public void Undo()
    {
        if (storedChanges == null) return;

        int key = storedChanges.Last();
        storedChanges.RemoveAt(storedChanges.Count - 1);

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

    public void OpenScaleMaker(object sender, EventArgs e)
    {
        scaleMakerOpen = true;
        ResetChanges();
        gameObject.SetActive(true);
    }

    public void CloseScaleMaker(object sender, EventArgs e)
    {
        scaleMakerOpen = false;
        gameObject.SetActive(false);
    }
}
