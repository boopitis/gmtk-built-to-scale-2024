using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject titleScreenGO;

    private void Start()
    {
        titleScreenGO.SetActive(true);
    }

    public void PlayGame()
    {
        GameManager.Instance.LoadScene(GameManager.Scene.Main);
    }
}
