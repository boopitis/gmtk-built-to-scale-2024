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

    public static void SpawnProjectile(GameObject notePrefab, Transform transform, Quaternion rotation,
        out Projectile projectile)
    {
        var gameObject = Instantiate(notePrefab, transform.position, rotation);
        projectile = gameObject.GetComponent<Projectile>();
        var projectile_rb = gameObject.GetComponent<Rigidbody2D>();
        projectile_rb.AddForce(transform.right * projectile.projectileSpeed, ForceMode2D.Impulse);
    }

    public static void SpawnProjectile(GameObject notePrefab, Transform transform, Quaternion rotation,
        Quaternion fireDirection, out Projectile projectile)
    {
        var gameObject = Instantiate(notePrefab, transform.position, rotation);
        projectile = gameObject.GetComponent<Projectile>();
        var projectile_rb = gameObject.GetComponent<Rigidbody2D>();
        
        projectile_rb.AddForce(fireDirection * transform.right * projectile.projectileSpeed, ForceMode2D.Impulse);
    }

    public int SetPiercing(int piercing) => this.piercing = piercing;
}
