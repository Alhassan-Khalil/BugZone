using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputHandler : MonoBehaviour
{

    #region Old

    private PlayerInput inputs;

    [Header("Sub Behaviours")]
    public PlayerMovementO playerMovement;
    public PlayerAnimation playerAnimation;


    [Header("Input Settings")]
    public PlayerInput playerInput;
    public float movementSmoothingSpeed = 1f;
    public Vector2 RawInputMovement { get; private set; }
    public Vector2 smothInputMovement { get; private set; }
    private Vector2 rawInputMouse;
    private Vector2 smothInputMovementVelocity;


    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float jumpTimer;

    public bool JumpInput;
    public bool JumpInputStop;
    public bool ToggleCrouch;
    public bool HoldCrouch;
    public bool ToggleSprint;
    public bool isWalking;


    //Action Maps
    //private string actionMapPlayerControls = "Player Controls";


    //Current Control Scheme
    private string currentControlScheme;

    private void Start()
    {
        jumpTimer = -1;
    }
    private void FixedUpdate()
    {
        UpdatePlayerMovement();
    }

    private void Update()
    {
        //CalculateMovementInputSmoothing();
        UpdatePlayerMouse();
        //UpdatePlayerAnimationMovement();
        CheckJumpInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 inputMovement = context.ReadValue<Vector2>();
            //rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
            RawInputMovement = new Vector2(inputMovement.x, inputMovement.y);
            //smothInputMovement =Vector2.SmoothDamp(rawInputMovement, inputMovement, ref smothInputMovementVelocity,0.2f);
            isWalking = true;
        }
        if (context.canceled)
        {
            RawInputMovement = Vector2.zero;
            isWalking = false;
        }
    }

    public void UpdatePlayerAnimationMovement()
    {
        if (playerMovement.Sliding)
        {
            playerAnimation.slide();
        }
        if (!isWalking && !ToggleSprint)
        {
            playerAnimation.updateMovement(0f);
        }
        else if (isWalking && !ToggleSprint)
        {
            playerAnimation.updateMovement(0.5f);
        }
        else if (isWalking && ToggleSprint)
        {
            playerAnimation.updateMovement(1f);
        }
    }

    public void OnMouseInput(InputAction.CallbackContext context)
    {
        Vector2 inputMouse = context.ReadValue<Vector2>();
        rawInputMouse = inputMouse;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpTimer = Time.time;
            //playerMovement.OnJump(JumpInput);
            jumpTimer++;
        }
        if (context.performed)
        {
        }
        if (context.canceled)
        {
            JumpInput = false;
            JumpInputStop = true;
            //playerMovement.OnJump(JumpInput);
            playerAnimation.jump(false);
        }
    }
    public void OnCrouchInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToggleCrouch = true;
            //playerMovement.OnCrouch(ToggleCrouch);
            playerAnimation.Crouch(true);
        }
        if (context.performed)
        {
            HoldCrouch = true;
        }
        if (context.canceled)
        {
            ToggleCrouch = false;
            HoldCrouch = false;

            //playerMovement.OnCrouch(ToggleCrouch);
            playerAnimation.Crouch(false);
        }
    }
    public void OnSprintInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToggleSprint = true;
            //playerMovement.OnSprint(ToggleSprint);
        }
        if (context.performed)
        {
        }
        if (context.canceled)
        {
            ToggleSprint = false;
            //playerMovement.OnSprint(ToggleSprint);
        }
    }

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpTimer + inputHoldTime)
        {
            JumpInput = false;
        }
    }
    public void ResetJump()
    {
        jumpTimer = -1;
    }

    public void UseJumpInput() => JumpInput = false;


/*    void CalculateMovementInputSmoothing()
    {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }*/

    void UpdatePlayerMovement()
    {
        //playerMovement.UpdateMovementData(rawInputMovement);
    }
    void UpdatePlayerMouse()
    {

        //playerMovement.UpdateMouseLook(rawInputMouse);

    }


    #endregion


}
