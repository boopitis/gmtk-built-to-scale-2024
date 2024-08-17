using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Gun Instance { get; private set; }
    
    private SpriteRenderer characterRenderer;
    private SpriteRenderer weaponRenderer;
    private Vector2 PointerPosition { get; set; }

    private const float Delay = 0.3f;
    private bool attackBlocked;

    private GameObject projectilePrefab;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private Transform firePoint;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (Player.Instance.GetLookDirection().x < 0)
        {
            scale.y = -1;
            scale.x = -1;
        }
        else if (Player.Instance.GetLookDirection().x > 0)
        {
            scale.y = 1;
            scale.x = 1;
        }
        transform.localScale = scale;

        if (transform.eulerAngles.z is > 0 and < 180)
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
        yield return new WaitForSeconds(Delay);
        attackBlocked = false;
    }

    public void SetPointerPosition(Vector2 vector2)
    {
        PointerPosition = vector2;
    }
}
