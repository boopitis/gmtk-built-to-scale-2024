using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ScaleViewer : MonoBehaviour
{
    public static ScaleViewer Instance { get; private set; }

    [SerializeField] private GameObject uiPanel;

    [SerializeField] private List<GameObject> panels;

    [SerializeField] private List<ScaleSO> createdScaleSOList;
    private int count = 0;

    private void Start()
    {
        Instance = this;

        uiPanel.SetActive(false);

        GameManager.Instance.OnPause += GameManager_OnPause;
        GameManager.Instance.OnResume += GameManager_OnResume;
        PlayerMusicScaleManager.Instance.OnScaleChanged += CreateScalePanel;
    }

    public void CreateScalePanel(object sender, PlayerMusicScaleManager.OnScaleCreatedEventArgs e)
    {
        // Creates an info panel of new found scales in the pause menu UI
        if (!createdScaleSOList.Contains(e.ScaleSO))
        {
            // foreach (NoteSO noteSOs in newCreatedScaleSO.noteSOList)
            // {
            //     print(noteSOs.name);
            // }
            // foreach (ScaleSO createdScaleSO in createdScaleSOList)
            // {
            //     print(createdScaleSO.name);
            // }
            print("Create New Panel");
            createdScaleSOList.Add(e.ScaleSO);
            panels[count].transform.GetChild(0).GetComponent<TMP_Text>().text = e.ScaleSO.name;
            panels[count].transform.GetChild(1).GetComponent<TMP_Text>().text = e.ScaleSO.specialDescription;
            panels[count].transform.GetChild(2).GetComponent<TMP_Text>().text = e.ScaleSO.passiveDescription;

            count++;
        }
    }

    public void GameManager_OnPause(object sender, GameManager.OnPauseEventArgs e)
    {
        if (e.PauseCondition != GameManager.PauseCondition.InputActivation) return;
        uiPanel.SetActive(true);
    }

    public void GameManager_OnResume(object sender, EventArgs e)
    {
        uiPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}