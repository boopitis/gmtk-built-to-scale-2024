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
        var activeScaleSOSpecials = PlayerMusicScale.Instance.GetScaleSpecialsNeedingFiring(noteIndex);

        Debug.Log(noteIndex); //DEBUG
        
        if (activeScaleSOSpecials.Count == 0) // Fire normal note
        {
            Debug.Log(firedNoteSO.name); //DEBUG
            Projectile.SpawnProjectile(firedNoteSO.prefab, firePointTransform, transform.rotation, out _);
        }
        else // Fire special(s)
        {
            foreach (var scaleSO in activeScaleSOSpecials)
            {
                Debug.Log(scaleSO.name); //DEBUG
                scaleSO.special.Fire(firePointTransform, transform.rotation);
            }
        }

        noteIndex++;
        if (noteIndex == PlayerMusicScale.Instance.GetCurrentNoteSOList().Count) noteIndex = 0;
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(ShootDelay);
        attackBlocked = false;
    }
}