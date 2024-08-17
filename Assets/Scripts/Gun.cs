using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Gun Instance { get; private set; }

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform firePoint;
    
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private SpriteRenderer weaponRenderer;

    private Vector2 pointerPositionInput;

    private const float Delay = 0.3f;
    private static readonly Vector2 WindowOffset = new Vector2(Screen.width, Screen.height);
    private bool attackBlocked;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        pointerPositionInput = GameInput.Instance.GetPlayerPointerPositionVector2();
        Vector2 direction = (pointerPositionInput - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (PlayerVisual.Instance.GetLookDirection().x < 0)
        {
            scale.y = -1;
            scale.x = -1;
        }
        else if (PlayerVisual.Instance.GetLookDirection().x > 0)
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
}
