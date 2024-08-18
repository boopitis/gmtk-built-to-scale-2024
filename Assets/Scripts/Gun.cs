using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum Special
{
    Major,
    MelodicMinor
}
public class Gun : MonoBehaviour
{
    public static Gun Instance { get; private set; }

    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private SpriteRenderer weaponRenderer;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform firePoint;

    private Vector2 pointerPositionInput;

    [SerializeField] private float shootDelay = 0.3f;
    private static readonly Vector2 WindowOffset = new Vector2(Screen.width, Screen.height);
    private bool attackBlocked;

    [SerializeField] private Special special;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    private GameObject[] projectilePrefabs;
    private int note = 0;
    [SerializeField]
    private Scale musicScale;
    private int interval;

    private void Update()
    {
        pointerPositionInput = GameInput.Instance.GetPlayerPointerPositionVector2InWorldSpace();
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
        if (attackBlocked)
            return;

        attackBlocked = true;
        StartCoroutine(DelayAttack());

        if (note == 12)
        {
            switch (special)
            {
                case Special.Major:
                    MajorShotgun();
                    break;
                case Special.MelodicMinor:
                    MelodicMinorBigBullet();
                    break;
            }
        }
        else
        {
            GameObject projectile = Instantiate(projectilePrefabs[note], firePoint.position, transform.rotation);
            Rigidbody2D projectile_rb = projectile.GetComponent<Rigidbody2D>();
            projectile_rb.AddForce(firePoint.right * projectileSpeed, ForceMode2D.Impulse);

            NextNote();
        }
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(shootDelay);
        attackBlocked = false;
    }

    private void MajorShotgun()
    {
        int bullets = 8;
        int spread = 30;

        note = 0;
        interval = 0;
        for (int i = 0; i < bullets; i++)
        {
            GameObject projectile = Instantiate(projectilePrefabs[note], firePoint.position, Quaternion.Euler(0, 0, -spread + (spread * 2 / bullets * i)) * transform.rotation);
            Rigidbody2D projectile_rb = projectile.GetComponent<Rigidbody2D>();
            projectile_rb.AddForce(Quaternion.Euler(0, 0, -spread + (spread * 2 / (bullets - 1) * i)) * firePoint.right * projectileSpeed, ForceMode2D.Impulse);

            NextNote();
        }
        note = 0;
        interval = 0;
    }

    private void MelodicMinorBigBullet()
    {
        GameObject projectile = Instantiate(projectilePrefabs[note], firePoint.position, transform.rotation);
        projectile.transform.localScale *= 8f;
        projectile.GetComponent<Projectile>().piercing = 20;
        Rigidbody2D projectile_rb = projectile.GetComponent<Rigidbody2D>();
        projectile_rb.AddForce(firePoint.right * projectileSpeed, ForceMode2D.Impulse);

        NextNote();
    }

    private void NextNote()
    {
        note += musicScale.intervals[interval];
        if (note > 12)
            note -= 13;

        interval++;
        if (interval > musicScale.intervals.Length - 1)
            interval = 0;
    }
}