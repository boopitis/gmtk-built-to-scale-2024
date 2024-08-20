using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Scale", menuName = "Scriptable Objects/New Scale")]
public class ScaleSO : ScriptableObject
{
    public GameObject prefab;
    public BaseSpecial special;
    public float specialAnimationDelay;
    public NoteSO[] noteSOList;
    [FormerlySerializedAs("name")] public string scaleName;
    [TextArea(4, 1)] public string specialDescription;
    [TextArea(4, 1)] public string passiveDescription;
}