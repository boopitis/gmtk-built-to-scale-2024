using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private GameObject menuCanvas;

    private bool isPaused = false;

    private void Start()
    {
        menuCanvas.SetActive(false);

        GameInput.Instance.OnPlayerMenuOpenClosePerformed += GameInput_OnPlayerMenuOpenClosePerformed;
    }

    private void Update()
    {

    }

    private void GameInput_OnPlayerMenuOpenClosePerformed(object sender, EventArgs e)
    {
        if (isPaused)
        {
            print("Menu Close");
            Time.timeScale = 1f;
            isPaused = false;
            menuCanvas.SetActive(false);
        }
        else
        {
            print("Menu Open");
            Time.timeScale = 0f;
            isPaused = true;
            menuCanvas.SetActive(true);
        }
    }
}
