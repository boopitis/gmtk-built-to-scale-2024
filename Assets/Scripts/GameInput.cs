using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnPlayerShootPerformed;
    public event EventHandler OnUINavigatePerformed;
    public event EventHandler OnUISubmitPerformed;
    public event EventHandler OnUICancelPerformed;
    public event EventHandler OnUIClickPerformed;
    public event EventHandler OnUIMiddleClickPerformed;
    public event EventHandler OnUIRightClickPerformed;
    
    private InputActions inputActions;
    private Vector2 movementVector, pointerPositionVector;
    
    private void Awake()
    {
        Instance = this;

        inputActions = new InputActions();
        
        inputActions.Player.Enable();
        // Get Player Movement from GetPlayerMovementVector2()
        // Get Player PointerPosition from GetPlayerPointerPositionVector2()
        inputActions.Player.Shoot.performed += PlayerShoot_performed;
        
        inputActions.UI.Enable();
        inputActions.UI.Navigate.performed += UINavigate_performed;
        inputActions.UI.Submit.performed += UISubmit_performed;
        inputActions.UI.Cancel.performed += UICancel_performed;
        // Get UI Point from GetUIPointVector2()
        inputActions.UI.Click.performed += UIClick_performed;
        // Get UI ScrollWheel from GetUIScrollWheelVector2()
        inputActions.UI.MiddleClick.performed += UIMiddleClick_performed;
        inputActions.UI.RightClick.performed += UIRightClick_performed;
        // Get UI TrackedDevicePosition from GetUITrackedDevicePositionVector3()
        // Get UI TrackedDeviceOrientation from GetUITrackedDeviceOrientationQuaternion()
    }

    public Quaternion GetUITrackedDeviceOrientationQuaternion()
    {
        return inputActions.UI.TrackedDeviceOrientation.ReadValue<Quaternion>();
    }

    public Vector3 GetUITrackedDevicePositionVector3()
    {
        return inputActions.UI.TrackedDevicePosition.ReadValue<Vector3>();
    }

    private void UIRightClick_performed(InputAction.CallbackContext obj)
    {
        OnUIRightClickPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void UIMiddleClick_performed(InputAction.CallbackContext obj)
    {
        OnUIMiddleClickPerformed?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetUIScrollWheelVector2()
    {
        return inputActions.UI.ScrollWheel.ReadValue<Vector2>();
    }

    private void UIClick_performed(InputAction.CallbackContext obj)
    {
        OnUIClickPerformed?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetUIPointVector2()
    {
        return inputActions.UI.Point.ReadValue<Vector2>();
    }

    private void UICancel_performed(InputAction.CallbackContext obj)
    {
        OnUICancelPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void UISubmit_performed(InputAction.CallbackContext obj)
    {
        OnUISubmitPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void UINavigate_performed(InputAction.CallbackContext obj)
    {
        OnUINavigatePerformed?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetPlayerMovementVector2()
    {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetPlayerPointerPositionVector2()
    {
        return inputActions.Player.PointerPosition.ReadValue<Vector2>();
    }

    private void PlayerShoot_performed(InputAction.CallbackContext obj)
    {
        OnPlayerShootPerformed?.Invoke(this, EventArgs.Empty);
    }
}
