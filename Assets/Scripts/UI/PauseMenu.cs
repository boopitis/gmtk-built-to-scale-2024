using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject MusicScaleMakerGO;
    [SerializeField] private GameObject MusicScaleSelectorGO;

    public void OpenMusicScaleMaker()
    {
        MusicScaleMakerGO.SetActive(true);
        MusicScaleSelectorGO.SetActive(false);
    }

    public void OpenMusicScaleSelector()
    {
        MusicScaleMakerGO.SetActive(false);
        MusicScaleSelectorGO.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
