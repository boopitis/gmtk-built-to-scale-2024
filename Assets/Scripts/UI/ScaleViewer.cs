using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ScaleViewer : MonoBehaviour
{
    public static ScaleViewer Instance { get; private set; }

    [SerializeField] private List<GameObject> panels;
    private int count = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateScalePanel(ScaleSO scaleSO)
    {
        panels[count].transform.GetChild(0).GetComponent<TMP_Text>().text = scaleSO.specialDescription;

        panels[count].transform.GetChild(1).GetComponent<TMP_Text>().text = scaleSO.passiveDescription;

        count++;
    }
}
