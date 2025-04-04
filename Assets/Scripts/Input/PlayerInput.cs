using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, InputActions.IGameplayActions
{
    public static event UnityAction onMoveLeft = delegate {};
    public static event UnityAction onMoveRight = delegate {};
    public static event UnityAction onDrop = delegate {};
    public static event UnityAction onCancelDrop = delegate {};
    public static event UnityAction onRotate = delegate {};

    public static bool keepMoveLeft = false;
    public static bool keepMoveRight = false;
    public static bool keepDrop = false;

    const float BUTTON_HOLD_TIME = 0.4f;

    WaitForSeconds waitForButtonHoldTime = new WaitForSeconds(BUTTON_HOLD_TIME);

    static InputActions inputActions;

    void Awake()
    {
        inputActions = new InputActions();
        inputActions.Gameplay.SetCallbacks(this);
    }

    void OnEnable()
    {
        EnableGameplayInputs();
    }

    void OnDisable()
    {
        DisableAllInputs();
    }

    public static void EnableGameplayInputs()
    {
        inputActions.Gameplay.Enable();
    }

    public static void DisableAllInputs()
    {
        inputActions.Gameplay.Disable();
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onMoveLeft.Invoke();
            StartCoroutine(nameof(KeepMoveLeftCoroutine));
        }

        if (context.canceled)
        {
            StopCoroutine(nameof(KeepMoveLeftCoroutine));
            keepMoveLeft = false;
        }
    }

    IEnumerator KeepMoveLeftCoroutine()
    {
        yield return waitForButtonHoldTime;

        keepMoveLeft = true;
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onMoveRight.Invoke();
            StartCoroutine(nameof(KeepMoveRightCoroutine));
        }

        if (context.canceled)
        {
            StopCoroutine(nameof(KeepMoveRightCoroutine));
            keepMoveRight = false;
        }
    }

    IEnumerator KeepMoveRightCoroutine()
    {
        yield return waitForButtonHoldTime;

        keepMoveRight = true;
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDrop.Invoke();
            StartCoroutine(nameof(KeepDropCoroutine));
        }

        if (context.canceled)
        {
            StopCoroutine(nameof(KeepDropCoroutine));
            keepDrop = false;
        }
    }
    
    IEnumerator KeepDropCoroutine()
    {
        yield return waitForButtonHoldTime;

        keepDrop = true;
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onRotate.Invoke();
        }
    }
}