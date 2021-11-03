using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{



    private bool isgrounded;
    private bool jumpinput;
    private bool isJumping;
    private bool jumpInputStop;
    protected Vector3 dir;
    protected Vector3 input;


    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
        isgrounded = player.grounded;
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

        input = player.InputHandler.MovementInput;
        jumpinput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;

        player.RB.drag = playerData.airDrag;

        CheckJumpMultiplayier();

        if (isgrounded && player.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (jumpinput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (player.InputHandler.Sprint)
        {
            player.RB.AddForce(dir.normalized * playerData.moveSpeed * playerData.movementMultiplier * playerData.airMultiplierSprint, ForceMode.Acceleration);
        }
        else
        {
            player.RB.AddForce(dir.normalized * playerData.moveSpeed * playerData.movementMultiplier * playerData.airMultiplier, ForceMode.Acceleration);
        }
    }
    private void CheckJumpMultiplayier()
    {

        if (isJumping)
        {
            if (jumpInputStop)
            {
                //player.setVelocityY(player.CurrentVelocity.y * playerData.jumpHiightMultiplier);
                isJumping = false;
            }
            else if( player.CurrentVelocity.y <= 0)
            {
                isJumping = false;
            }
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        dir = player.orientation.right * input.x + player.orientation.forward * input.y;


    }
    public void setIsJumping() => isJumping = true;




}
