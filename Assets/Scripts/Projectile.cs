using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [FormerlySerializedAs("projectileSpeed")][SerializeField] private float speed;
    [FormerlySerializedAs("projectilePrefab")][SerializeField] private GameObject prefab;
    [FormerlySerializedAs("projectileDamage")][SerializeField] private int damage;
    [SerializeField] private int piercing;
    [SerializeField] private GameObject hitEffect;

    private Collider2D lastHit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == lastHit) return;

        if (other.GetComponent<Health>())
        {
            other.GetComponent<Health>().GetHit(damage, gameObject);
            lastHit = other;
        }

        if (piercing == 0 || other.gameObject.layer == 6)
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
        projectile_rb.AddForce(transform.right * projectile.speed, ForceMode2D.Impulse);
    }

    public static void SpawnProjectile(GameObject notePrefab, Transform transform, Quaternion rotation,
        Quaternion fireDirection, out Projectile projectile)
    {
        var gameObject = Instantiate(notePrefab, transform.position, rotation);
        projectile = gameObject.GetComponent<Projectile>();
        var projectile_rb = gameObject.GetComponent<Rigidbody2D>();

        projectile_rb.AddForce(fireDirection * transform.right * projectile.speed, ForceMode2D.Impulse);
    }

    public GameObject GetPrefab() => prefab;
}
