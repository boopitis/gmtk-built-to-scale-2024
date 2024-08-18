using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteProjectile : Projectile
{
    // Range from 0-11; 0 is lowest C, 11 is B
    [SerializeField] private int pitch;

    private void Awake()
    {
        if (pitch is < 0 or > 11)
        {
            Debug.LogError("Invalid pitch in Note class!");
        }
    }

    public int GetPitch() => pitch;
}
