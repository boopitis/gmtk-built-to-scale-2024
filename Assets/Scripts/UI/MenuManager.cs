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

    private bool isPaused;
    private bool scaleMakerOpen;

    public event EventHandler OnWaveEnded;

    private void Awake()
    {
        isPaused = false;
        scaleMakerOpen = false;
    }

    private void Start()
    {
        Instance = this;

        OnWaveEnded += ToggleScaleMaker;
        GameInput.Instance.OnPlayerMenuOpenClosePerformed += ToggleScaleViewer;

        MusicScaleMakerPanelGO.SetActive(false);
        MusicScaleViewerPanelGO.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnWaveEnded?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ToggleScaleViewer(object sender, EventArgs e)
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
            MusicScaleViewerPanelGO.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            isPaused = true;
            MusicScaleViewerPanelGO.SetActive(true);
        }
    }

    public void ToggleScaleMaker(object sender, EventArgs e)
    {
        if (scaleMakerOpen)
        {
            Time.timeScale = 1f;
            scaleMakerOpen = false;
            MusicScaleMakerPanelGO.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            scaleMakerOpen = true;
            MusicScaleMaker.Instance.ResetChanges();
            MusicScaleMakerPanelGO.SetActive(true);
        }
    }
}
