using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scale", menuName = "Scriptable Objects/New Scale")]
public class ScaleSO : ScriptableObject
{
    public NoteSO[] noteSOList;
    public string name;
}
