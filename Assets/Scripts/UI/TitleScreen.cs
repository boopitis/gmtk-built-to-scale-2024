using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject titleScreenGO;
    [SerializeField] private GameObject creditsPanelGO;

    private void Start()
    {
        titleScreenGO.SetActive(true);
        creditsPanelGO.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenCredits()
    {
        titleScreenGO.SetActive(false);
        creditsPanelGO.SetActive(true);
    }

    public void ExitCredits()
    {
        titleScreenGO.SetActive(true);
        creditsPanelGO.SetActive(false);
    }
}
