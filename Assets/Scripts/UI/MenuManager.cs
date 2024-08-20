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

    private void Start()
    {
        menuCanvasGO.SetActive(false);
        MusicScaleMakerGO.SetActive(false);
        MusicScaleViewerGO.SetActive(true);

        GameManager.Instance.OnPause += GameManager_OnPause;
        GameManager.Instance.OnResume += GameManager_OnResume;
    }

    private void GameManager_OnResume(object sender, EventArgs e)
    {
        menuCanvasGO.SetActive(false);
        MusicScaleMakerGO.SetActive(false);
        MusicScaleViewerGO.SetActive(true);
    }

    private void GameManager_OnPause(object sender, EventArgs e)
    {
        menuCanvasGO.SetActive(true);
    }
}
