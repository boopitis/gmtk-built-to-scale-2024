using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int projectileDamage;
    public int piercing;

    [SerializeField]
    private GameObject hitEffect;

    private Collider2D lastHit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == lastHit) return;

        if (other.GetComponent<Health>())
        {
            other.GetComponent<Health>().GetHit(projectileDamage, gameObject);
            lastHit = other;
        }

        if (piercing == 0 || other.gameObject.layer == 11)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
            Destroy(gameObject);
        }

        piercing -= 1;
    }
}
