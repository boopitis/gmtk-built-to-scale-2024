using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    
    private CharacterAnimations characterAnimations;
    private Movement movement;

    private Gun gun;

    private Vector2 PointerInput { get; set; }

    private Vector2 MovementInput { get; set; }

    private Vector2 LookDirection { get; set; }
    
    private void Awake()
    {
        Instance = this;

        characterAnimations = GetComponentInChildren<CharacterAnimations>();
        gun = GetComponentInChildren<Gun>();
        movement = GetComponent<Movement>();
    }
    public void Shoot()
    {
        gun?.Attack();
    }

    private void Update()
    {
        Gun.Instance.SetPointerPosition(PointerInput);
        movement.MovementInput = MovementInput;
        AnimateCharacter();
    }

    private void AnimateCharacter()
    {
        LookDirection = PointerInput - (Vector2)transform.position;
        characterAnimations.RotateToPointer(LookDirection);
        // TODO: characterAnimations.PlayAnimation(MovementInput);
    }

    public Vector2 GetLookDirection()
    {
        return LookDirection;
    }
}
