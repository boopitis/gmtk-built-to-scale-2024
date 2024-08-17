using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class Gun : MonoBehaviour
{
    public static Gun Instance { get; private set; }
    
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private SpriteRenderer weaponRenderer;

    [SerializeField] private Transform firePointTransform;

    [SerializeField] private GameObject[] projectilePrefabs;
    [SerializeField] private ScaleSO musicScaleSO;

    private Vector2 pointerPositionInput;

    private const float Delay = 0.3f;
    private bool attackBlocked;

    private int note;
    private int interval;

    private void Awake()
    {
        Instance = this;
        
        note = 0;
        
        // Check if intervals is an octave
        var totalInterval = musicScaleSO.intervals.Sum();
        if (totalInterval != 12)
        {
            Debug.LogError("Current ScaleSO does not form an octave!");
        }
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

        print(note);
        Projectile.SpawnProjectile(projectilePrefabs[note], firePointTransform, transform.rotation);

        note += musicScaleSO.intervals[interval];
        if (note > 11)
            note -= 12;

        interval++;
        if (interval > musicScaleSO.intervals.Length - 1)
            interval = 0;
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(Delay);
        attackBlocked = false;
    }
}