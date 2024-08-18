using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Note", menuName = "Scriptable Objects/New Note")]
public class NoteSO : ScriptableObject
{
    // Range from 0-11; 0 is lowest C, 11 is B
    public int pitch;
    [FormerlySerializedAs("name")] public string noteName;
}
