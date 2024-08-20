using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseSpecial : MonoBehaviour
{
    [SerializeField] protected Projectile projectile;

    public abstract void Fire(Transform position, Quaternion rotation);
}
