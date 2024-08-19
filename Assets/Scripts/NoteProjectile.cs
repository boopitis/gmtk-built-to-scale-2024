using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteProjectile : Projectile
{
    [SerializeField] private NoteSO noteSO;

    private void Awake()
    {
        if (noteSO.pitch is < 0 or > 11)
        {
            Debug.LogError("Invalid pitch in Note class!");
        }
    }

    public NoteSO GetNoteSO() => noteSO;
}