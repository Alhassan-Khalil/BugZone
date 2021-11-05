using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerSlidingState SlidingState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerWallrunState wallrunState { get; private set; }
    public CapsuleCollider PlayerCollider { get; private set; }



    [SerializeField]
    private PlayerData playerData = default;
    #endregion

    #region Components

    public InputHandlerA InputHandler { get; private set; }
    public Rigidbody RB { get; private set; }
    public Animator Anim { get; private set; }
    public bool CanSetVelcity { get; set; }
    public bool IsNearWall { get; private set; }
    public bool IsWallRight { get; private set; }
    public bool IsWallLeft { get; private set; }

    public RaycastHit leftWallHit;
    public RaycastHit rightWallHit;


    public Vector3 CurrentVelocity { get; private set; }



    private Vector3 workspace;

    [Header("Mouse Look Settings")]
    [SerializeField] public Transform playerCamera;
    [SerializeField] public Transform orientation;


    private float xRotation = 0f;
    private Vector2 MouseInput;


    [Header("Ground Check Settings")]
    [SerializeField] private float slopeRaycastDistance = 1f;
    [SerializeField] private LayerMask groundLayer = default;
    [SerializeField] private LayerMask whatIsWall = default;

    public bool grounded;

    public float currentSlope=0f;
    public RaycastHit slopeHit;
    #endregion

    #region unity callback fun
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CanSetVelcity = true;
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "jump");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        CrouchState = new PlayerCrouchState(this, StateMachine, playerData, "crouch");
        SlidingState = new PlayerSlidingState(this, StateMachine, playerData, "slide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        wallrunState = new PlayerWallrunState(this , StateMachine, playerData,"wallRun");

    }

    private void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        InputHandler = GetComponent<InputHandlerA>();
        RB = GetComponent<Rigidbody>();
        PlayerCollider = GetComponent<CapsuleCollider>();
        
        
        StateMachine.Initialize(IdleState);

    }
    private void Update()
    {
        CurrentVelocity = RB.velocity;
        UpdateMouseLook();
        CheckForWall();
        StateMachine.CurrentState.logicUpdate();

    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion


    #region set Velocity

    public void SetVelocityTest(Vector3 velocity)
    {
        //RB.AddForce(velocity);
        workspace.Set(velocity.x,CurrentVelocity.y,velocity.z);
        SetFinalVelocity();
    }
    public void SetVelocity(float velocity, Vector3 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity, angle.z * velocity * direction);
        SetFinalVelocity();

    }
    public void SetVelocity0()
    {
        workspace = Vector2.zero;
        SetFinalVelocity();
    }

    public void setVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity, CurrentVelocity.z);
        SetFinalVelocity();
    }

    private void SetFinalVelocity()
    {
        if (CanSetVelcity)
        {
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }
    }


    #endregion

    #region Check fun 
    private void OnCollisionStay(Collision other)
    {
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            for (int i = 0; i < other.contactCount; i++)
            {
                if (Mathf.Round(other.GetContact(i).normal.y) == 1.0f)
                {
                    grounded = true;
                    break;
                }
            }

            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, slopeRaycastDistance, groundLayer);
            currentSlope = Vector3.Angle(Vector3.up, hit.normal);

        }
            /*            Vector3 forward = -orientation.TransformDirection(Vector3.forward) * 10;
                        if(!grounded && orientation.position.y > 2f)
                        {
                            if (InputHandler.JumpInput)
                            {
                                Debug.DrawRay(orientation.position, forward, Color.green,1.25f);
                                StateMachine.ChangeState(WallJumpState);
                            }
                        }*/
            if (!grounded)
            {
                IsNearWall = true;
            }
            else
                IsNearWall = false;
    }
    private void OnCollisionExit(Collision other)
    {
        grounded = false;
        IsNearWall = false;
    }



    private void OnTriggerStay(Collider other)
    {
        if (!grounded)
        {
            IsNearWall = true;
            Debug.Log(IsNearWall);
        }
        else
            IsNearWall = false;
    }

    private void OnTriggerExit(Collider other)
    {
        IsNearWall = false;
    }

    private void CheckForWall()
    {
        IsWallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, playerData.wallDistance);
        IsWallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, playerData.wallDistance);

    }
    public bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, playerData.minimumJumpHeight);
    }
    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position,Vector3.down,out slopeHit, PlayerCollider.height/ 2 + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }


    #endregion

    #region other fun 
    public void SetColliderHeight(float height)
    {
        Vector3 center = PlayerCollider.center;

        center.y += (height - PlayerCollider.height) / 2;

        PlayerCollider.height = height;
        PlayerCollider.center = center;
    }


    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();


    public void UpdateMouseLook()
    {
        MouseInput = InputHandler.MousetInput;

        MouseInput = MouseInput * playerData.sensitivity * Time.fixedDeltaTime * playerData.sensMultiplier;
        var rot = playerCamera.localRotation.eulerAngles;
        float xTo = rot.y + MouseInput.x;

        xRotation -= MouseInput.y;
        xRotation = Mathf.Clamp(xRotation, -playerData.maxAngle, playerData.maxAngle);


        playerCamera.localRotation = Quaternion.Euler(xRotation, xTo, 0f);
        orientation.localRotation = Quaternion.Euler(0f, xTo, 0f);
    }
    public void slidstop()
    {
        StartCoroutine(StopProjectedSlide(RB.velocity));
    }
    private IEnumerator StopProjectedSlide(Vector3 momentum)
    {
        //f*ing stupid f*ing physics dumb dumb math go BRRRR
        Vector3 velocity = momentum / RB.mass; //find velocity after slide
        Vector3 finalPos = transform.position + velocity; //estimated final position
        float distToPos = Vector3.Distance(transform.position, finalPos); //distance between final position and current position

        if (playerData.debugSlideTrajectory) Debug.DrawLine(transform.position, finalPos, Color.blue, 5f);

        while (InputHandler.Crouch && distToPos > playerData.slideStopThreshold && currentSlope < playerData.maxSlope)
        {
            distToPos = Vector3.Distance(transform.position, finalPos); // update distance to final position

            if (playerData.debugSlideTrajectory)
                print($"Distance to final pos: {distToPos} | Arrived: {distToPos < playerData.slideStopThreshold}");

            yield return null;
        }

        SlidingState.setSlide();
    }


    #endregion  
}
