using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{

    protected Vector3 input;
    protected Vector3 dir;
    protected Vector3 slopMoveDir;

    protected bool isSprinting;
    protected bool OnCrouch;
    protected bool onSlope;

    private bool jumpInput;
    protected bool isgrounded;


    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
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
        player.JumpState.RestAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void logicUpdate()
    {
        base.logicUpdate();

        input = player.InputHandler.MovementInput;
        jumpInput = player.InputHandler.JumpInput;
        isSprinting = player.InputHandler.Sprint;
        onSlope = player.OnSlope();
        OnCrouch = player.InputHandler.Crouch;

        slopMoveDir = Vector3.ProjectOnPlane(dir, player.slopeHit.normal);

        if(isgrounded && OnCrouch)
        {
            stateMachine.ChangeState(player.CrouchState);
        }
        if(isgrounded && OnCrouch && isSprinting)
        {
            stateMachine.ChangeState(player.SlidingState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            player.InputHandler.useJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isgrounded)
        {
            player.JumpState.DecreaseAmountOfJumpLeft();
            stateMachine.ChangeState(player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        dir = player.orientation.right * input.x + player.orientation.forward * input.y;
    }
}
