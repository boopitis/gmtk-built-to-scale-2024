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
    private Vector2 pointerInput;
    private Vector2 movementInput;
    private Vector2 lookDirection;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        characterAnimations = GetComponentInChildren<CharacterAnimations>();
        movement = GetComponent<Movement>();
    }

    public void Shoot()
    {
        Gun.Instance?.Attack();
    }

    private void Update()
    {
        Gun.Instance.SetPointerPosition(pointerInput);
        movement.SetMovementInput(movementInput);
        AnimateCharacter();
    }

    private void AnimateCharacter()
    {
        lookDirection = pointerInput - (Vector2)transform.position;
        characterAnimations.RotateToPointer(lookDirection);
        // TODO: characterAnimations.PlayAnimation(MovementInput);
    }

    public Vector2 GetLookDirection()
    {
        return lookDirection;
    }
}
