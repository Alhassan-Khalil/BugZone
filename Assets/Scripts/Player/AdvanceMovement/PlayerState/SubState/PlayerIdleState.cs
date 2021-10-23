using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
    {
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityTest(Vector3.zero);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void logicUpdate()
    {
        base.logicUpdate();
        if (input != Vector3.zero)
        {
            stateMachine.ChangeState(player.MoveState);
        }
        else if (OnCrouch)
        {
            stateMachine.ChangeState(player.CrouchState);
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }








}
