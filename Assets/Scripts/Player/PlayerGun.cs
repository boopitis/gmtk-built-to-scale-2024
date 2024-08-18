using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerGun : MonoBehaviour
{
    public static PlayerGun Instance { get; private set; }
    
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private SpriteRenderer weaponRenderer;

    [SerializeField] private Transform firePointTransform;

    [SerializeField] private ScaleSO musicScaleSO;

    private Vector2 pointerPositionInput;

    private const float ShootDelay = 0.3f;
    private bool attackBlocked;

    private int noteIndex;
    [SerializeField] private GameObject debugSpecialProjectile;
    [SerializeField] private Special debugSpecial;
    private enum Special
    {
        Major,
        MelodicMinor
    }
    
    private void Awake()
    {
        Instance = this;
        
        noteIndex = 0;
    }
    
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
        if (attackBlocked) return;

        attackBlocked = true;
        StartCoroutine(DelayAttack());

        var firedNoteSO = PlayerMusicScale.Instance.GetCurrentNoteSOList()[noteIndex];

        Debug.Log(noteIndex); //DEBUG
        Debug.Log(firedNoteSO.name); //DEBUG

        noteIndex++;
        if (noteIndex != PlayerMusicScale.Instance.GetCurrentNoteSOList().Count)
        {
            Projectile.SpawnProjectile(firedNoteSO.prefab, firePointTransform, transform.rotation, out _);
            return;
        }
        
        noteIndex = 0;
        switch (debugSpecial)
        {
            default:
            case Special.Major:
                MajorShotgun();
                break;
            case Special.MelodicMinor:
                MelodicMinorBigBullet();
                break;
        }
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(ShootDelay);
        attackBlocked = false;
    }

    private void MajorShotgun()
    {
        const int bullets = 8;
        const int spread = 30;

        for (int i = 0; i < bullets; i++)
        {
            Projectile.SpawnProjectile(
                debugSpecialProjectile, 
                firePointTransform, 
                Quaternion.Euler(0, 0, -spread + (spread * 2.0f / bullets * i)) * transform.rotation,
                Quaternion.Euler(0, 0, -spread + (spread * 2.0f / (bullets - 1) * i)),
                out _);
        }
    }

    private void MelodicMinorBigBullet()
    {
        Projectile.SpawnProjectile(debugSpecialProjectile, firePointTransform, transform.rotation, out var projectile);
        projectile.transform.localScale *= 8f;
        projectile.SetPiercing(20);
    }
}