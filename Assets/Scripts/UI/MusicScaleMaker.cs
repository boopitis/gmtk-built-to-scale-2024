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

    [SerializeField] private GameObject UIpanel;
    [SerializeField] private GameObject[] keyImages;
    [SerializeField] private NoteSO[] noteSOS;
    [SerializeField] private int allowedChanges = 3;
    private int changes;
    private List<int> storedChanges;

    public event EventHandler OnWaveEnded;

    public event EventHandler StartNextWave;

    public event EventHandler OnConfirmation;

    private void Start()
    {
        Instance = this;

        UIpanel.SetActive(false);

        OnWaveEnded += OpenScaleMaker;
        StartNextWave += CloseScaleMaker;
        GameManager.Instance.OnPause += GameManager_OnPause;
        GameManager.Instance.OnResume += GameManager_OnResume;

        changes = allowedChanges;
        storedChanges = new List<int>();
    }

    // Debug Section
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnWaveEnded?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OpenScaleMaker(object sender, EventArgs e)
    {
        GameManager.Instance.Pause(GameManager.PauseCondition.EndOfWave);
    }

    public void CloseScaleMaker(object sender, EventArgs e)
    {
        GameManager.Instance.Resume();
    }

    public void NextWave()
    {
        // Debug
        StartNextWave?.Invoke(this, EventArgs.Empty);

        OnConfirmation?.Invoke(this, EventArgs.Empty);
    }
    // End Debug Section

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
        if (!storedChanges.Any()) return;

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

    public void GameManager_OnPause(object sender, GameManager.OnPauseEventArgs e)
    {
        if (e.PauseCondition != GameManager.PauseCondition.EndOfWave) return;
        changes = allowedChanges;
        UIpanel.SetActive(true);
    }

    public void GameManager_OnResume(object sender, EventArgs e)
    {
        storedChanges = new List<int>();
        UIpanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}