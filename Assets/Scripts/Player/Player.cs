using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CharacterAnimations characterAnimations;
    private Movement movement;

    private Gun gun;

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    public Vector2 lookDirection { get; private set; }

    public void Shoot()
    {
        gun?.Attack();
    }

    private void Awake()
    {
        characterAnimations = GetComponentInChildren<CharacterAnimations>();
        gun = GetComponentInChildren<Gun>();
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        gun.PointerPosition = pointerInput;
        movement.MovementInput = movementInput;
        AnimateCharacter();
    }

    private void AnimateCharacter()
    {
        lookDirection = pointerInput - (Vector2)transform.position;
        characterAnimations.RotateToPointer(lookDirection);
        // TODO: characterAnimations.PlayAnimation(MovementInput);
    }
}
