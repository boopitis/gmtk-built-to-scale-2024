using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private Movement movement;
    private UnityEngine.Vector2 movementInput;
    private void Awake()
    {
        Instance = this;

        movement = GetComponent<Movement>();
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerShootPerformed += GameInput_OnPlayerShootPerformed;
    }
    
    private void Update()
    {
        movementInput = GameInput.Instance.GetPlayerMovementVector2();
        movement.SetMovementInput(movementInput);
    }

    private void GameInput_OnPlayerShootPerformed(object sender, EventArgs e)
    {
        Gun.Instance?.Attack();
    }

    public UnityEngine.Vector2 GiveC()
    {
        
        UnityEngine.Vector2 coords = (UnityEngine.Vector2)transform.position;
        return coords;
    }
}
