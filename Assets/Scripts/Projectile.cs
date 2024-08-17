using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectileDamage;
    [SerializeField] private int piercing;
    [SerializeField] private GameObject hitEffect;

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

    public static void SpawnProjectile(GameObject notePrefab, Transform transform, Quaternion rotation)
    {
        var gameObject = Instantiate(notePrefab, transform.position, rotation);
        var projectile_rb = gameObject.GetComponent<Rigidbody2D>();
        var projectile = gameObject.GetComponent<Projectile>();
        projectile_rb.AddForce(transform.right * projectile.projectileSpeed, ForceMode2D.Impulse);
    }
}
