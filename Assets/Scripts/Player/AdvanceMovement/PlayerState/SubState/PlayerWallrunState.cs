using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallrunState : PlayerAbiltyState
{

    public PlayerWallrunState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
        player.Anim.SetBool("wallRun", true);
        player.RB.useGravity = false;
        isWallRunning = true;
        player.JumpState.RestAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
        player.RB.useGravity = true;
        isWallRunning = false;
        player.Anim.SetBool("wallRun", false);
    }

    public override void logicUpdate()
    {
        base.logicUpdate();

        if(!IsWallRight && !IsWallleft)
        {
            IsAbilityDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.RB.AddForce(Vector3.down * playerData.wallRunGravity, ForceMode.Force);

        if (JumpInput)
        {
            if (IsWallleft)
            {
                Vector3 wallRunJumpDirection = player.transform.up + player.leftWallHit.normal;
                player.RB.velocity = new Vector3(player.RB.velocity.x, 0, player.RB.velocity.z);
                player.RB.AddForce(wallRunJumpDirection * playerData.wallRunJumpForce * 100, ForceMode.Force);
            }
            else if (IsWallRight)
            {
                Vector3 wallRunJumpDirection = player.transform.up + player.rightWallHit.normal;
                player.RB.velocity = new Vector3(player.RB.velocity.x, 0, player.RB.velocity.z);
                player.RB.AddForce(wallRunJumpDirection * playerData.wallRunJumpForce * 100, ForceMode.Force);
            }
        }
        else
        {
            playerData.moveSpeed = Mathf.Lerp(playerData.moveSpeed, playerData.sprintSpeed, playerData.acceleration * Time.deltaTime);
            player.RB.AddForce(dir.normalized * playerData.moveSpeed * playerData.movementMultiplier, ForceMode.Acceleration);
        }

    }
}
