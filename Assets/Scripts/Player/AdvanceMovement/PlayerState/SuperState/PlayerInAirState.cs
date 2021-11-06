using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{



    private bool isgrounded;
    private bool jumpinput;
    private bool isJumping;
    private bool jumpInputStop;
    private bool IsWallRight;
    private bool IsWallleft;

    protected bool isNearWall;


    protected Vector3 dir;
    protected Vector3 input;


    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
        isgrounded = player.grounded;
        isNearWall = player.IsNearWall;
        IsWallRight = player.IsWallRight;
        IsWallleft = player.IsWallLeft;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        isNearWall = false;
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
        else if (IsWallleft || IsWallRight)
        {
            stateMachine.ChangeState(player.wallrunState);
        }
/*        else if (jumpinput && isNearWall)
        {
            stateMachine.ChangeState(player.WallJumpState);
        }*/
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
                player.RB.AddForce(player.orientation.up * playerData.jumpHiightMultiplier, ForceMode.Impulse);

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
