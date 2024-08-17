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
    public UnityEvent OnShoot, OnMelee, OnToggleMenu;

    [SerializeField]
    private InputActionReference movement, shoot, meleeAttack, pointerPosition, openInventory, closeInventory;

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
        meleeAttack.action.performed += Melee;
        openInventory.action.performed += OpenInventory;
        closeInventory.action.performed += CloseInventory;
    }

    private void OnDisable()
    {
        shoot.action.performed -= Shoot;
        meleeAttack.action.performed -= Melee;
        openInventory.action.performed -= OpenInventory;
        closeInventory.action.performed -= CloseInventory;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        OnShoot?.Invoke();
    }

    private void Melee(InputAction.CallbackContext context)
    {
        OnMelee?.Invoke();
    }

    private void OpenInventory(InputAction.CallbackContext context)
    {
        playerInput.SwitchCurrentActionMap("Menu");
        OnToggleMenu?.Invoke();
    }

    private void CloseInventory(InputAction.CallbackContext context)
    {
        playerInput.SwitchCurrentActionMap("InGame");
        OnToggleMenu?.Invoke();
    }
}
