using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void logicUpdate()
    {
        base.logicUpdate();

        if (!IsExitingState)
        {

            if (input == Vector3.zero)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (OnCrouch)
            {
                stateMachine.ChangeState(player.CrouchState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (isSprinting)
        {
            player.SetVelocityTest(playerData.sprintMultiplier * dir);
            player.Anim.SetFloat("movement", 1f,0.1f, Time.deltaTime);
        }
        else if(onSlope && isgrounded)
        {
            player.SetVelocityTest(playerData.moveMultiplier * slopMoveDir);

        }
        else
        {
            player.SetVelocityTest(playerData.moveMultiplier * dir);
            player.Anim.SetFloat("movement", 0f, 0.1f, Time.deltaTime);
        }




    }
}
