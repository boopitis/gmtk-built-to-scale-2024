using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnPause;
    public event EventHandler OnResume;
    
    private bool isPaused;

    private void Awake()
    {
        Instance = this;

        isPaused = false;
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerMenuOpenClosePerformed += GameInput_OnPlayerMenuOpenClosePerformed;
    }

    private void GameInput_OnPlayerMenuOpenClosePerformed(object sender, EventArgs e)
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            OnPause?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnResume?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsPaused() => isPaused;
}