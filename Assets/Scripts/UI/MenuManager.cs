using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private GameObject menuCanvasGO;
    [SerializeField] private GameObject MusicScaleMakerGO;
    [SerializeField] private GameObject MusicScaleViewerGO;

    private bool isPaused = false;

    private void Start()
    {
        menuCanvasGO.SetActive(false);
        MusicScaleMakerGO.SetActive(false);
        MusicScaleViewerGO.SetActive(true);

        GameInput.Instance.OnPlayerMenuOpenClosePerformed += GameInput_OnPlayerMenuOpenClosePerformed;
    }

    private void GameInput_OnPlayerMenuOpenClosePerformed(object sender, EventArgs e)
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
            menuCanvasGO.SetActive(false);
            MusicScaleMakerGO.SetActive(false);
            MusicScaleViewerGO.SetActive(true);
        }
        else
        {
            Time.timeScale = 0f;
            isPaused = true;
            menuCanvasGO.SetActive(true);
        }
    }
}
