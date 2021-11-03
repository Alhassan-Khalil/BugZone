using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlidingState : PlayerGroundedState
{
    public PlayerSlidingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
        Sliding = true;
        player.SetColliderHeight(playerData.crouchheight);

        Debug.Log("i am sliding");
    }

    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(playerData.Standheight);
    }

    public override void logicUpdate()
    {
        base.logicUpdate();

            if (!Sliding)
            {

                if (OnCrouch )
                {
                    stateMachine.ChangeState(player.CrouchState);
                }
                else if (input != Vector3.zero)
                {
                    stateMachine.ChangeState(player.MoveState);
                }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.RB.AddForce(player.orientation.forward * playerData.slideForce, ForceMode.Impulse);
        timeSinceLastSlide = 0f;
        //Sliding = false;
        player.slidstop();
    }

    public void setSlide()
    {
        Sliding = false;
        Debug.Log("i am false");
    }
}
