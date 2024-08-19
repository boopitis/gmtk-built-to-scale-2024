using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Note", menuName = "Scriptable Objects/New Note")]
public class NoteSO : ScriptableObject
{
    public GameObject prefab;
    // Range from 0-12; 0 is lowest C, 12 is highest C
    public int pitch;
    [FormerlySerializedAs("name")] public string noteName;
}
