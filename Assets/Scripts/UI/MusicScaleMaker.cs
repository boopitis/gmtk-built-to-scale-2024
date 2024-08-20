using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MusicScaleMaker : MonoBehaviour
{
    public static MusicScaleMaker Instance { get; private set; }

    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject[] keyImages;
    [SerializeField] private NoteSO[] noteSOS;
    [SerializeField] private int allowedChanges = 3;
    [SerializeField] private GameObject changesLeftText;
    private int changes;
    private List<int> storedChanges;

    public event EventHandler OnConfirmation;

    private void Start()
    {
        Instance = this;

        uiPanel.SetActive(false);

        FindObjectOfType<SpawnDude>().OnWaveEnded += OpenScaleMaker;
        GameManager.Instance.OnPause += GameManager_OnPause;
        GameManager.Instance.OnResume += GameManager_OnResume;

        changes = allowedChanges;
        storedChanges = new List<int>();
        UpdateChangesLeftText();
    }

    public void OpenScaleMaker(object sender, EventArgs e)
    {
        GameManager.Instance.Pause(GameManager.PauseCondition.EndOfWave);
    }

    public void NextWave()
    {
        GameManager.Instance.Resume();
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
            UpdateChangesLeftText();
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
        UpdateChangesLeftText();
    }

    public void GameManager_OnPause(object sender, GameManager.OnPauseEventArgs e)
    {
        if (e.PauseCondition != GameManager.PauseCondition.EndOfWave) return;
        changes = allowedChanges;
        UpdateChangesLeftText();
        uiPanel.SetActive(true);
    }

    public void GameManager_OnResume(object sender, EventArgs e)
    {
        storedChanges = new List<int>();
        uiPanel.SetActive(false);
    }

    public void UpdateChangesLeftText()
    {
        changesLeftText.GetComponent<TMP_Text>().text = new string("Changes Left: " + changes);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
