using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbiltyState : PlayerState
{
    protected bool IsAbilityDone;
    protected bool isGrounded;

    public PlayerAbiltyState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
        isGrounded = player.grounded;
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
    }
}

