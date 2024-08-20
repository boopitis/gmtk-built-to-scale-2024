using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class MusicScaleMaker : MonoBehaviour
{
    [SerializeField] private GameObject[] keyImages;
    [SerializeField] private NoteSO[] noteSOS;

    public void ToggleKey(int key)
    {
        if (keyImages[key].activeInHierarchy)
        {
            keyImages[key].SetActive(false);
            PlayerMusicScaleManager.Instance.TryRemoveNote(noteSOS[key]);
        }
        else
        {
            keyImages[key].SetActive(true);
            PlayerMusicScaleManager.Instance.TryAddNote(noteSOS[key]);
        }
    }
}
