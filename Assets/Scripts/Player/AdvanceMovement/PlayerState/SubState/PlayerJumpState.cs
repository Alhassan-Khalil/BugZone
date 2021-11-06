using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbiltyState
{

    private int amountOfJumpLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
    {
        amountOfJumpLeft = playerData.amountOfJump;
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.useJumpInput();
        Jump();
        IsAbilityDone = true;
        amountOfJumpLeft--;
        player.InAirState.setIsJumping(); 
    }
    public bool CanJump()
    {
        if (amountOfJumpLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public void Jump()
    {
        player.RB.velocity = new Vector3(player.RB.velocity.x, 0, player.RB.velocity.z);
        player.RB.AddForce(player.orientation.up * playerData.jumpForce, ForceMode.Impulse);

        Vector3 vel = player.RB.velocity;
        if (player.RB.velocity.y < 0.5f)
            player.RB.velocity = new Vector3(vel.x, 0, vel.z);
        else if (player.RB.velocity.y > 0)
            player.RB.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
    }
    public void RestAmountOfJumpsLeft() => amountOfJumpLeft = playerData.amountOfJump;

    public void DecreaseAmountOfJumpLeft() => amountOfJumpLeft--;
}
