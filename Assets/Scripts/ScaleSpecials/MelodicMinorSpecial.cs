using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodicMinorSpecial : BaseSpecial
{
    /**
     * Big bullet attack
     */
    public override void Fire(Transform position, Quaternion rotation)
    {
        Projectile.SpawnProjectile(projectile.gameObject, position, rotation, out _);
    }
}
