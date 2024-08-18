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

    private Vector2 pointerPositionInput;
    private int noteIndex;
    
    private void Awake()
    {
        Instance = this;
        
        noteIndex = 0;
    }

    private void Start()
    {
        BeatManager.Instance.OnCurrentSubdivisionChange += BeatManager_OnCurrentSubdivisionChange;
    }

    private void BeatManager_OnCurrentSubdivisionChange(object sender, BeatManager.OnCurrentSubdivisionChangeEventArgs e)
    {
        Attack(e.CurrentSubdivision);
    }

    private void Update()
    {
        UpdateFacingDirection();
    }

    private void UpdateFacingDirection()
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

    public void Attack(int currentSubdivision)
    {
        var firedNoteSO = PlayerMusicScaleManager.Instance.GetCurrentNoteSOList()[noteIndex];
        var activeScaleSOSpecials = PlayerMusicScaleManager.Instance.GetScaleSpecialsNeedingFiring(noteIndex);
        
        if (activeScaleSOSpecials.Count == 0) // Fire normal note
        { 
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
        if (noteIndex == PlayerMusicScaleManager.Instance.GetCurrentNoteSOList().Count) noteIndex = 0;
    }
}