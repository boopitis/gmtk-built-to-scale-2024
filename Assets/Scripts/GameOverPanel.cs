using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private float timeBeforeShow; // modify to allow for death animation
    [SerializeField] private GameObject gameover;

    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        mainMenuButton?.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadScene(GameManager.Scene.TitleScreen);
        });

        Player.Instance.gameObject.GetComponent<Health>().OnDeath += Player_OnDeath;
    }

    private void Update()
    {
        timeBeforeShow -= Time.deltaTime;

        if (timeBeforeShow > 0) return;

        Time.timeScale = 0;
        Show();
    }

    private void Player_OnDeath(object sender, EventArgs e)
    {
        GameManager.Instance.Pause(GameManager.PauseCondition.PlayerDead);
        Time.timeScale = 1f;

        Show();
    }

    private void Show()
    {
        gameover.SetActive(true);
    }

    private void Hide()
    {
        gameover.SetActive(false);
    }
}
