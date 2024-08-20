using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : EnemyDamage
{
    private void FixedUpdate()
    {
        if (phealth != null)
        {
            base.OnCollisionEnter2D(col);
        }
    }
}

