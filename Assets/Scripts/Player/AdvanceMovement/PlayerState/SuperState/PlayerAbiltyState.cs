using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbiltyState : PlayerState
{
    protected Vector3 dir;
    protected Vector3 input;


    protected bool IsAbilityDone;
    protected bool isWallRunning;
    protected bool isGrounded;
    protected bool IsWallRight;
    protected bool IsWallleft;
    protected bool JumpInput;
    public PlayerAbiltyState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
        isGrounded = player.grounded;
        IsWallRight = player.IsWallRight;
        IsWallleft = player.IsWallLeft;

    }

    public override void Enter()
    {
        base.Enter();
        IsAbilityDone = false;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void logicUpdate()
    {
        base.logicUpdate();
        JumpInput = player.InputHandler.JumpInput;
        input = player.InputHandler.MovementInput;


        if (IsAbilityDone)
        {
            if (isGrounded && player.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        dir = player.orientation.right * input.x + player.orientation.forward * input.y;

    }
}

