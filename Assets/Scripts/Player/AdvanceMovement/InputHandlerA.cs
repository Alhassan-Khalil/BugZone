using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandlerA : MonoBehaviour
{
    public Vector2 MovementInput { get; private set; }
    public Vector3 MoveDirection { get; private set; }
    public Vector2 MousetInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool Sprint { get; private set; }
    public bool Crouch { get; private set; }

    private float jumpInputStartTime;

    [SerializeField]
    private float inputHoldTime = 0.2f;



    private void Update()
    {
        CheckJumpInputHoldTime();
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }
    public void OnMouseInput(InputAction.CallbackContext context)
    {
        MousetInput = context.ReadValue<Vector2>();
    }
    public void OnSprintInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Sprint = true;
        }
        if (context.canceled)
        {
            Sprint = false;
        }
    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnCroch(InputAction.CallbackContext context)
    {
        Crouch = context.performed;
    }


    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    public void useJumpInput() => JumpInput = false;
}
