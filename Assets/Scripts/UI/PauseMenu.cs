using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public event EventHandler StartNextWave;

    public void NextWave()
    {
        MenuManager.Instance.ToggleScaleMaker(this, EventArgs.Empty);
        StartNextWave?.Invoke(this, EventArgs.Empty);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
