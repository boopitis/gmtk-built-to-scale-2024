using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluesSpecial : BaseSpecial
{
    [SerializeField] private Collider2D projectileCollider;
    
    private const int Raycasts = 4;
    private const int Spread = 11;
    
    /** 
     * Laser attack
     */
    public override void Fire(Transform position, Quaternion rotation)
    {
        RaycastHit2D[] collisions = {  };
   
        for (int i = 0; i < Raycasts; i++)
        {

            projectileCollider.GetComponent<Collider2D>().Raycast(
                Quaternion.Euler(0, 0, -Spread + (Spread * 2.0f / Raycasts * i)) * rotation * Vector2.right,
                collisions,
                5.5f);
            Projectile.SpawnProjectile(
                projectile.gameObject, 
                position, 
                Quaternion.Euler(0, 0, -Spread + (Spread * 2.0f / Raycasts * i)) * rotation,
                Quaternion.Euler(0, 0, -Spread + (Spread * 2.0f / (Raycasts - 1) * i)),
                out _);

            string debugOut = "hit points: ";
            foreach (var raycastHit2D in collisions)
            {
                debugOut += raycastHit2D.point + ", ";
            }
            Debug.Log(debugOut);
        }
    }
}
