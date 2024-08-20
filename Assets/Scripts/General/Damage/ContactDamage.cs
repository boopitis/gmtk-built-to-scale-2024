using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : EnemyDamage
{
    private void FixedUpdate()
    {
        if (phealth is not null)
        {
            base.OnCollisionEnter2D(col);
        }
    }
}

