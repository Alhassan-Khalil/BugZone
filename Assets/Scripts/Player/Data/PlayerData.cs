using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newPlayerDate", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{





    [Header("Movement")]
    [SerializeField] public float moveSpeed = 6f;
    [SerializeField] public float airMultiplier = 0.4f;
    [SerializeField] public float airMultiplierSprint = 0.8f;
    [SerializeField] public float maxSlope = 15f;

    [SerializeField] public float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] public float walkSpeed = 4f;
    [SerializeField] public float sprintSpeed = 6f;
    [SerializeField] public float acceleration = 10f;

    [Header("Jumping")]
    [SerializeField] public float jumpForce = 5f;


    [Header("Drag")]
    [SerializeField] public float groundDrag = 6f;
    [SerializeField] public float airDrag = 2f;


    [Header("Slide Settings")]
    [SerializeField] public bool enableSlide = true;
    [SerializeField] public bool debugSlideTrajectory = false;
    [SerializeField] public float slideForce = 25f;
    [SerializeField] public float slideFriction = 3f;
    [SerializeField] public float slideCooldown = 1f;
    [SerializeField] public float slideSpeedThreshold = 5f;
    [SerializeField] public float slideStopThreshold = 2.4f;
    #region adv
    //[SerializeField] public float moveSpeed = 350f;
    //[SerializeField] public float movementMultiplier = 9f;
    //[SerializeField] public float airMultiplier = 10f;
    //[SerializeField] public float airMultiplierSprint = 15;
    [SerializeField] public float crouchheight = 1.5f;
    [SerializeField] public float Standheight = 2.044783f;
    [SerializeField] public float crouchMoveMultiplier = 0.5f;
    [SerializeField] public bool enableInAirDrag;

    //[SerializeField] public float jumpForce = 7f;
    [SerializeField] public int amountOfJump = 2;
    [SerializeField] public float jumpHiightMultiplier = 1.5f;
    #endregion



    [Header("Movement Settings")]
    [SerializeField] public float groundMaxSpeed = 20f;
    [SerializeField] public float friction = 230f;
    [SerializeField] public float Graviry = 10f;

    [Space(15)]
    //[SerializeField] public float airDrag = 160f;
    [SerializeField] public float inAirMaxSpeed = 30f;

    [Header("Sprint Settings")]
    [SerializeField] public float sprintMultiplier = 20f;
    [SerializeField] [Tooltip("Adds to the original max speed.")] public float sprintMaxSpeedModifier = 8f;



    [Header("Crouch Settings")]
    [SerializeField] public float crouchMaxSpeed = 5f;
    [SerializeField] public float crouchJumpMultiplier = 1.4f;



    [Header("Mouse Look Settings")]
    [SerializeField] public Vector2 sensitivity = new Vector2(20f, 20f);
    [SerializeField] public float sensMultiplier = 0.2f;
    [SerializeField] public float maxAngle = 90f;

}
