using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private Movement movement;
    private Vector2 movementInput;

    public Vector2 coords;

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
        // PlayerGun.Instance?.Attack();
    }

    public Vector2 GiveC()
    {
        coords = transform.position;
        return coords;
    }
}
