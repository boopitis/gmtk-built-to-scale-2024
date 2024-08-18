using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class MusicScaleMaker : MonoBehaviour
{
    [SerializeField] private GameObject[] keyImages;

    public void ToggleKey(int key)
    {
        if (keyImages[key].activeInHierarchy)
            keyImages[key].SetActive(false);
        else
            keyImages[key].SetActive(true);
    }
}
