using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 PointerPosition { get; set; }

    public float delay = 0.3f;
    private bool attackBlocked;

    public GameObject projectilePrefab;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private Transform firePoint;

    private void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (gameObject.GetComponentInParent<Player>().lookDirection.x < 0)
        {
            scale.y = -1;
            scale.x = -1;
        }
        else if (gameObject.GetComponentInParent<Player>().lookDirection.x > 0)
        {
            scale.y = 1;
            scale.x = 1;
        }
        transform.localScale = scale;

        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }

    public void Attack()
    {
        print(attackBlocked);
        if (attackBlocked)
            return;

        attackBlocked = true;
        StartCoroutine(DelayAttack());

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, transform.rotation);
        Rigidbody2D projectile_rb = projectile.GetComponent<Rigidbody2D>();
        projectile_rb.AddForce(firePoint.right * projectileSpeed, ForceMode2D.Impulse);

    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }
}
