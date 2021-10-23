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
        player.jump();
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
    public void RestAmountOfJumpsLeft() => amountOfJumpLeft = playerData.amountOfJump;

    public void DecreaseAmountOfJumpLeft() => amountOfJumpLeft--;
}
