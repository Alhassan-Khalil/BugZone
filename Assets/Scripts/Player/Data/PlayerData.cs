using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newPlayerDate", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{

    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 350f;
    [SerializeField] public float moveMultiplier = 9f;
    [SerializeField] public float groundMaxSpeed = 20f;
    [SerializeField] public float friction = 230f;
    [SerializeField] public float maxSlope = 15f;
    [SerializeField] public float Graviry = 10f;

    [Space(15)]
    [SerializeField] public float inAirMaxSpeed = 30f;
    [SerializeField] public float inAirMovementModifier = 0.8f;
    [SerializeField] public float inAirDrag = 160f;

    [Header("Sprint Settings")]
    [SerializeField] public float sprintMultiplier = 20f;
    [SerializeField] [Tooltip("Adds to the original max speed.")] public float sprintMaxSpeedModifier = 8f;

    [Header("Jump Settings")]
    [SerializeField] public float jumpForce = 7f;
    [SerializeField] public float jumpMultiplier = 1.5f;
    [SerializeField] public int amountOfJump = 2;


    [Header("Crouch Settings")]
    [SerializeField] public float crouchMoveMultiplier = 0.5f;
    [SerializeField] public float crouchMaxSpeed = 5f;
    [SerializeField] public float crouchJumpMultiplier = 1.4f;
    [SerializeField] public float crouchheight = 1.5f;


    [Header("Slide Settings")]
    [SerializeField] public float slideForce = 25f;
    [SerializeField] public float slideFriction = 3f;
    [SerializeField] public float slideCooldown = 1f;
    [SerializeField] public float slideSpeedThreshold = 5f;
    [SerializeField] public float slideStopThreshold = 2.4f;

    [Header("Mouse Look Settings")]
    [SerializeField] public Vector2 sensitivity = new Vector2(20f, 20f);
    [SerializeField] public float sensMultiplier = 0.2f;
    [SerializeField] public float maxAngle = 90f;

}