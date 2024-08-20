using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum PauseCondition
    {
        EndOfWave,
        InputActivation
    }

    public event EventHandler<OnPauseEventArgs> OnPause;
    public class OnPauseEventArgs : EventArgs
    {
        public PauseCondition PauseCondition;
    }

    private PauseCondition pauseCondition;

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
        if (!isPaused)
        {
            Pause(PauseCondition.InputActivation);
            return;
        }

        if (pauseCondition != PauseCondition.InputActivation) return;

        Resume();
    }

    public bool IsPaused() => isPaused;

    public PauseCondition GetPauseCondition() => pauseCondition;

    public void Pause(PauseCondition pauseCondition)
    {
        isPaused = true;
        Time.timeScale = 0f;
        this.pauseCondition = pauseCondition;
        OnPause?.Invoke(this, new OnPauseEventArgs
        {
            PauseCondition = this.pauseCondition
        });
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        OnResume?.Invoke(this, EventArgs.Empty);
    }
}