using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Scale", menuName = "Scriptable Objects/New Scale")]
public class ScaleSO : ScriptableObject
{
    public NoteSO[] noteSOList;
    [FormerlySerializedAs("name")] public string scaleName;
}
