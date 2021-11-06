using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWall : PlayerState
{
    protected bool Isgrounded;
    protected bool IsTouchingWall;
    protected bool GrabInput;
    protected bool JumpuInput;
    protected bool IstouchingLedge;
    protected float xinput;
    protected float yinput;

    public PlayerTouchingWall(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname) : base(player, stateMachine, playerData, animBoolname)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();

        Isgrounded = player.grounded;

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
        xinput = player.InputHandler.MovementInput.x;
        yinput = player.InputHandler.MovementInput.y;
        //GrabInput = player.InputHandler.GrabInput;
        JumpuInput = player.InputHandler.JumpInput;


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
