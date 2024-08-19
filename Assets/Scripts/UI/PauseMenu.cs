using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject MusicScaleMakerGO;
    [SerializeField] private GameObject MusicScaleViewerGO;

    public void OpenMusicScaleMaker()
    {
        MusicScaleMakerGO.SetActive(true);
        MusicScaleViewerGO.SetActive(false);
    }

    public void OpenMusicScaleViewer()
    {
        MusicScaleMakerGO.SetActive(false);
        MusicScaleViewerGO.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
