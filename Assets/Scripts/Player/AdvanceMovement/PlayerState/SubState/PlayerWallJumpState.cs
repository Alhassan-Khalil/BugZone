using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbiltyState
{
    private int wallJumpDirection;

    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
/*        player.InputHandler.useJumpInput();
        player.JumpState.RestAmountOfJumpsLeft();
*//*        player.SetVelocity(playerData.WallJumpVelocity, playerData.WallJumpAngle, -wallJumpDirection);*//*
        player.JumpState.DecreaseAmountOfJumpLeft();*/
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void logicUpdate()
    {
        base.logicUpdate();
        GetDir();
        if (Time.time >= startTime + playerData.WallJumpTime)
        {
            IsAbilityDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }



    private void GetDir()
    {
        // Get the forward, backward, left and right directions of the player
        // NOTE: some directions not necessary if your player can only jump on
        // some of the directions in the array
        Vector3[] Directions = new Vector3[]
        {
                 player.orientation.forward,
                 player.orientation.forward,
                 player.orientation.right,
                 -player.orientation.right
        };


        foreach(Vector3 direction in Directions)
             {
            // Fire a small raycast out of the player to the wall in the given direction
            if (Physics.Raycast(player.orientation.position, direction, out RaycastHit hit, 1f))
            {
                // Add a force to the player in the direction of the wall's normal
                player.RB.AddForce(hit.normal * playerData.WallJumpVelocity * Time.deltaTime, ForceMode.Impulse);

                break;
            }
        }

    }

}
