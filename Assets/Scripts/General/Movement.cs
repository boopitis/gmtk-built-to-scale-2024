using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float deceleration = 1;
    
    private Rigidbody2D rb2d;
    private float currentSpeed;
    private Vector2 oldMovementInput;
    private Vector2 movementInput;

    private void Awake()
    {
        currentSpeed = 0;
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (movementInput.magnitude > 0 && currentSpeed >= 0)
        {
            oldMovementInput = movementInput;
            currentSpeed += acceleration * maxSpeed * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deceleration * maxSpeed * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        rb2d.velocity = oldMovementInput * currentSpeed;
    }

    public void SetMovementInput(Vector2 vector2)
    {
        movementInput = vector2;
    }
    
}
