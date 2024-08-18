using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Scale List", menuName = "Scriptable Objects/New Scale List")]
public class ScaleListSO : ScriptableObject
{
    public ScaleSO[] scaleSOs;
}
