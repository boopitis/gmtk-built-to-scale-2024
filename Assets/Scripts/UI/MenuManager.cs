using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private GameObject MusicScaleMakerPanelGO;
    [SerializeField] private GameObject MusicScaleViewerPanelGO;

    private bool scaleMakerOpen;

    public event EventHandler OnWaveEnded;

    private void Awake()
    {
        scaleMakerOpen = false;
    }

    private void Start()
    {
        Instance = this;

        OnWaveEnded += ToggleScaleMaker;
        GameManager.Instance.OnPause += GameManager_OnPause;
        GameManager.Instance.OnResume += GameManager_OnResume;

        MusicScaleMakerPanelGO.SetActive(false);
        MusicScaleViewerPanelGO.SetActive(false);
    }

    private void GameManager_OnResume(object sender, EventArgs eventArgs)
    {
        MusicScaleViewerPanelGO.SetActive(false);
    }

    private void GameManager_OnPause(object sender, GameManager.OnPauseEventArgs e)
    {
        if (e.PauseCondition != GameManager.PauseCondition.InputActivation) return;
        
        MusicScaleViewerPanelGO.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnWaveEnded?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ToggleScaleMaker(object sender, EventArgs e)
    {
        if (scaleMakerOpen)
        {
            GameManager.Instance.Resume();
            MusicScaleMakerPanelGO.SetActive(false);
        }
        else
        {
            GameManager.Instance.Pause(GameManager.PauseCondition.EndOfWave);
            MusicScaleMaker.Instance.ResetChanges();
            MusicScaleMakerPanelGO.SetActive(true);
        }
    }
}
