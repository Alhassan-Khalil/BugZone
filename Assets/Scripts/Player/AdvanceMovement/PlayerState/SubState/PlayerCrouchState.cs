using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerGroundedState
{
    public PlayerCrouchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();


        player.SetColliderHeight(playerData.crouchheight);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(playerData.Standheight);
    }

    public override void logicUpdate()
    {
        base.logicUpdate();

        if (!IsExitingState)
        {

            if (!OnCrouch)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //player.SetVelocityTest(playerData.crouchMoveMultiplier * dir);
        player.RB.AddForce(dir.normalized * playerData.moveSpeed * playerData.crouchMoveMultiplier, ForceMode.Acceleration);
    }
}
