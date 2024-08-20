using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHealth : Health
{
    public static event EventHandler OnAnyDeath;
    // Start is called before the first frame update
    public void GetHit(int amount, GameObject sender)
    {
        if (immune || isDead || sender.layer == gameObject.layer)
            return;

        health -= amount;

        if (health > 0)
        {
            InvokeOnHit();
            // because the base event can't be called in an inherited class.
        }
        else
        {
            OnAnyDeath?.Invoke(this, EventArgs.Empty);
            isDead = true;
            Destroy(gameObject);
        }

    }
}
