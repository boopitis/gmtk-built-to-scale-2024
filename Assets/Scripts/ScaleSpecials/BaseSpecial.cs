using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseSpecial : MonoBehaviour
{
    protected enum IndexToFire
    {
        First,
        Last,
        Middle,
        Second,
        SecondLast
    }
    
    [SerializeField] protected IndexToFire indexToFire;
    [SerializeField] protected Projectile projectile;

    public abstract void Fire(Transform position, Quaternion rotation);

    public bool IsNeedingFiring(int currentScaleLength, int index)
    {
        switch (indexToFire)
        {
            default:
            case IndexToFire.First:
                return index == 0;
            case IndexToFire.Last:
                return index == currentScaleLength - 1;
            case IndexToFire.Middle:
                return currentScaleLength / 2 == index;
            case IndexToFire.Second:
                return index == 1;
            case IndexToFire.SecondLast:
                return index == currentScaleLength - 2;
        }
    }
}
