using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScaleViewer : MonoBehaviour
{
    public static ScaleViewer Instance { get; private set; }

    [SerializeField] private List<ScaleSO> musicScaleSOList;
    [SerializeField] private List<GameObject> panels;
    private int count = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateScalePanel(int scaleIndex)
    {
        print(scaleIndex);
        panels[count].transform.GetChild(0).GetComponent<TMP_Text>().text = musicScaleSOList[scaleIndex].specialDescription;

        panels[count].transform.GetChild(1).GetComponent<TMP_Text>().text = musicScaleSOList[scaleIndex].passiveDescription;

        count++;
    }
}
