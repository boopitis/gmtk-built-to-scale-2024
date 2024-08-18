using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpecial : MonoBehaviour
{
    protected enum FireTime
    {
        First,
        Last,
        Middle,
        Second,
        SecondLast
    }
    
    [SerializeField] protected FireTime fireTime;
    [SerializeField] protected Projectile projectile;

    public abstract void Fire(Transform position, Quaternion rotation);

    public bool IsQueued(int createScaleLength, int currentNote)
    {
        switch (fireTime)
        {
            default:
            case FireTime.First:
                return currentNote == 0;
            case FireTime.Last:
                return currentNote == createScaleLength - 1;
            case FireTime.Middle:
                return createScaleLength / 2 == currentNote;
            case FireTime.Second:
                return currentNote == 1;
            case FireTime.SecondLast:
                return currentNote == createScaleLength - 2;
        }
    }
}
