using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Health>())
        {
            collision.collider.GetComponent<Health>().GetHit(damage, gameObject);
        }
    }
}
