using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool IsAnimationFinihed;
    protected float startTime;
    private string animBoolname;
    protected bool IsExitingState;
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolname)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolname = animBoolname;
    }

    public virtual void Enter()
    {
        DoCheck();
        player.Anim.SetBool(animBoolname, true);
        startTime = Time.time;
        Debug.Log(animBoolname);
        IsAnimationFinihed = false;
        IsExitingState = false;
    }


    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolname, false);
        IsExitingState = true;
    }

    public virtual void logicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoCheck();

    }
    public virtual void DoCheck()
    {

    }

    public virtual void AnimationTrigger()
    {

    }

    public virtual void AnimationFinishTrigger() => IsAnimationFinihed = true;

}

