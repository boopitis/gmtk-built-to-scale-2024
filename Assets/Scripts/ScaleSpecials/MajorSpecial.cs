using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorSpecial : BaseSpecial
{
    private const int Bullets = 8;
    private const int Spread = 30;
    
    /**
     * Shotgun attack
     */
    public override void Fire(Transform position, Quaternion rotation)
    {
        for (int i = 0; i < Bullets; i++)
        {
            Projectile.SpawnProjectile(
                projectile.gameObject, 
                position, 
                Quaternion.Euler(0, 0, -Spread + (Spread * 2.0f / Bullets * i)) * rotation,
                Quaternion.Euler(0, 0, -Spread + (Spread * 2.0f / (Bullets - 1) * i)),
                out _);
        }
    }
}
