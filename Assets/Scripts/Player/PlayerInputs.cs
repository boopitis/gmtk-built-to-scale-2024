using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    public UnityEvent OnShoot;

    [SerializeField]
    private InputActionReference movement, shoot, pointerPosition;

    private void Update()
    {
        OnMovementInput?.Invoke(movement.action.ReadValue<Vector2>().normalized);
        OnPointerInput?.Invoke(GetPointerInput());
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void OnEnable()
    {
        shoot.action.performed += Shoot;
    }

    private void OnDisable()
    {
        shoot.action.performed -= Shoot;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        OnShoot?.Invoke();
    }
}
