using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private Movement movement;
    
    private Vector2 playerPositionInput;
    private Vector2 movementInput;
    private Vector2 lookDirection;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerShootPerformed += GameInput_OnPlayerShootPerformed;
    }

    private void GameInput_OnPlayerShootPerformed(object sender, EventArgs e)
    {
        Gun.Instance?.Attack();
    }
    
    private void Update()
    {
        playerPositionInput = GameInput.Instance.GetPlayerPointerPositionVector2();

        movementInput = GameInput.Instance.GetPlayerMovementVector2();
        movement.SetMovementInput(movementInput);
    }
}
