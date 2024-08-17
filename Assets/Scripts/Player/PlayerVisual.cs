using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public static PlayerVisual Instance { get; private set; }
    
    [SerializeField] private CharacterAnimations characterAnimations;
    [SerializeField] private Player player;
    
    private Vector2 lookDirection;
    private Vector2 playerPositionInput;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        playerPositionInput = GameInput.Instance.GetPlayerPointerPositionVector2InWorldSpace();
        
        lookDirection = playerPositionInput - (Vector2)transform.position;
        characterAnimations.RotateToPointer(lookDirection);
        // TODO: characterAnimations.PlayAnimation(MovementInput);
    }

    public Vector2 GetLookDirection()
    {
        return lookDirection;
    }
}
