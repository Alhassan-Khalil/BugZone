using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementO : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData = default;
    //public PlayerAnimation playerAnimation;


    [Header("Component References")]
    public Rigidbody playerRigidbody;
    public CapsuleCollider playerCollider;

    [Header("Ground Check Settings")]
    [SerializeField] private float slopeRaycastDistance = 1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Mouse Look Settings")]
    [SerializeField] public Transform playerCamera;
    [SerializeField] public Transform orientation;

    [Header("Giving Control")]
    [SerializeField] private bool hasControl = true;
    [SerializeField] private bool enableInAirDrag = false;
    [SerializeField] private bool enableSprint = true;
    [SerializeField] private bool enableJump = true;
    [SerializeField] public bool autoJump = true;
    [SerializeField] private bool enableCrouch = true;
    [SerializeField] private bool enableSlide = true;
    [SerializeField] private bool enableMouseLook = true;
    [SerializeField] public bool debugSlideTrajectory = false;





    //Stored Values
    private float xRotation = 0f;
    private float orginalHeight = 0f;
    private float TimeSinceLastSlide = Mathf.Infinity;
    private float CurrentSlope = 0f;
    private int amountOfJumpLeft;
    private Vector2 fkingslide;

    //public bool JumpInputStop;
    public bool Jumping;
    public bool Crouching;
    public bool Sliding;
    public bool Sprinting;
    public bool grounded;


    private float CurSpeed
    {
        get
        {
            if (!enableSprint) return playerData.moveSpeed * playerData.movementMultiplier;
            return playerData.moveSpeed * (Sprinting ? playerData.sprintMultiplier : playerData.movementMultiplier);
        }
    }
    private float maxSpeed
    {
        get
        {
            if (!enableSprint) return (!grounded || Jumping) ? playerData.inAirMaxSpeed : (Crouching && grounded) ? playerData.crouchMaxSpeed : playerData.groundMaxSpeed;
            return (!grounded || Jumping) ? playerData.inAirMaxSpeed : (grounded && !Crouching && Sprinting) ? playerData.groundMaxSpeed + playerData.sprintMaxSpeedModifier : (Crouching && grounded) ? playerData.crouchMaxSpeed : playerData.groundMaxSpeed;
        }
    }


    private void Awake() => Cursor.lockState = CursorLockMode.Locked;
    private void Start() => orginalHeight = playerCollider.height;

    private void Update()
    {
        if (TimeSinceLastSlide < playerData.slideCooldown) TimeSinceLastSlide += Time.deltaTime;

    }
    public void UpdateMouseLook(Vector2 MouseInput)
    {
        if (!enableMouseLook) return;

        MouseInput = MouseInput *playerData.sensitivity * Time.fixedDeltaTime * playerData.sensMultiplier;
        var rot = playerCamera.localRotation.eulerAngles;
        float xTo = rot.y + MouseInput.x;

        xRotation -= MouseInput.y;
        xRotation = Mathf.Clamp(xRotation, -playerData.maxAngle, playerData.maxAngle);


        playerCamera.localRotation = Quaternion.Euler(xRotation, xTo, 0f);
        transform.localRotation = Quaternion.Euler(0f, xTo, 0f);
    }

    public void UpdateMovementData(Vector2 moveInput)
    {
        if (!hasControl) return;
        fkingslide = moveInput;
        Vector3 dir = orientation.right * moveInput.x + orientation.forward * moveInput.y;
        playerRigidbody.AddForce(Vector3.down * Time.fixedDeltaTime * playerData.Graviry);//add force down 

        if (autoJump && Jumping && grounded) OnJump(Jumping);
 
        if(Crouching && grounded && CurrentSlope >= playerData.maxSlope)
        {
            playerRigidbody.AddForce(Vector3.down * Time.fixedDeltaTime * 5000f);
            return;
        }

        float multiplier = grounded && Crouching ? playerData.crouchMoveMultiplier : 1f;

        if (playerRigidbody.velocity.magnitude > maxSpeed) dir = Vector3.zero;
        playerRigidbody.AddForce(GetMovementVector(-playerRigidbody.velocity, dir.normalized, CurSpeed * Time.fixedDeltaTime) * ((grounded && Jumping) ? multiplier : playerData.airMultiplier));

        //Debug.Log(fkingslide);
    }

    private Vector3 GetMovementVector(Vector3 velocity, Vector3 dir, float speed)
    {
        if (!grounded && velocity.magnitude != 0 && enableInAirDrag || velocity.magnitude != 0 && enableInAirDrag && Jumping)
        {
            float drop = playerData.airDrag * Time.fixedDeltaTime;
            velocity *= drop != 0f ? drop : 1f;

            return new Vector3(velocity.x, 0f, velocity.z) + dir * speed;
        }

        if (grounded && velocity.magnitude != 0f && Crouching && CurrentSlope >= playerData.maxSlope || grounded && Sliding && TimeSinceLastSlide < playerData.slideCooldown)
        {
            velocity *= playerData.slideFriction * Time.fixedDeltaTime;
            return velocity + dir * speed;
        }

        if (grounded && velocity.magnitude != 0f) velocity *= playerData.friction * Time.fixedDeltaTime;
        return velocity + dir * speed;
    }
    public void OnJump(bool JumpInput)
    {
        Jumping = JumpInput;
        if (!enableJump) return;

        if (grounded)
        {
            //If crouching and not sliding: crouch jump multiplier, if sliding: slide jump multiplier, and if all else is false: normal jump multiplier.
            float slideJumpMultiplier = playerRigidbody.velocity.y < 0 ? playerRigidbody.velocity.magnitude * 0.1f + playerData.jumpHiightMultiplier : playerData.crouchJumpMultiplier; //scales to speed
            float currentMultiplier = Crouching && CurrentSlope < playerData.maxSlope ? playerData.crouchJumpMultiplier : Crouching && CurrentSlope >= playerData.maxSlope ? slideJumpMultiplier : playerData.jumpHiightMultiplier;
            playerRigidbody.AddForce(Vector2.up * playerData.jumpForce * currentMultiplier, ForceMode.Impulse);
            grounded = false;
        }
    }
    public void OnCrouch(bool enabled)
    {
        if (!enableCrouch) return;

        if (enabled)
        {
            Crouching = true;
            SetColliderHeight(playerData.crouchheight);

            if (CanSlide()) Slide();
        }
        else
        {
            Crouching = false;
            Sliding = false;
            SetColliderHeight(orginalHeight);
        }
    }



    private void OnCollisionStay(Collision collision)
    {
        if(((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if(Mathf.Round(collision.GetContact(i).normal.y) == 1.0f)
                {
                    grounded = true;
                    break;
                }
            }
            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, slopeRaycastDistance, groundLayer);
            CurrentSlope = Vector3.Angle(Vector3.up, hit.normal);
        }
    }

    private void OnCollisionExit(Collision collision) => grounded = false;
    public void OnSprint(bool sprintinput) => Sprinting = sprintinput;


    public void SetColliderHeight(float height)
    {
        Vector3 center = playerCollider.center;

        center.y += (height - playerCollider.height) / 2;

        playerCollider.height = height;
        playerCollider.center = center;
    }



    private void Slide()
    {
        if (!enableSlide) return;

        playerRigidbody.AddForce(orientation.forward * playerData.slideForce, ForceMode.Impulse);
        Sliding = true;
        SetColliderHeight(playerData.crouchheight);
        TimeSinceLastSlide = 0f;

        StartCoroutine(StopProjectedSlide(playerRigidbody.velocity));
    }

    private IEnumerator StopProjectedSlide(Vector3 momentum)
    {
        //f*ing stupid f*ing physics dumb dumb math go BRRRR
        Vector3 velocity = momentum / playerRigidbody.mass; //find velocity after slide
        Vector3 finalPos = transform.position + velocity; //estimated final position
        float distToPos = Vector3.Distance(transform.position, finalPos); //distance between final position and current position

        if (debugSlideTrajectory) Debug.DrawLine(transform.position, finalPos, Color.blue, 5f);

        while (Crouching && distToPos > playerData.slideStopThreshold && CurrentSlope < playerData.maxSlope)
        {
            distToPos = Vector3.Distance(transform.position, finalPos); // update distance to final position

            if (debugSlideTrajectory)
                print($"Distance to final pos: {distToPos} | Arrived: {distToPos < playerData.slideStopThreshold}");

            yield return null;
        }
        Sliding = false;
        SetColliderHeight(orginalHeight);
    }

    private bool CanSlide()
    {
        if (fkingslide.y < 0f)
        {
            return false;
        }
        else
        {
            return playerRigidbody.velocity.magnitude > playerData.slideSpeedThreshold && grounded && Crouching && !Sliding && TimeSinceLastSlide >= playerData.slideCooldown && CurrentSlope < playerData.maxSlope;
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (CanSlide()) Slide();
    }





}
