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

            if (player.RB.velocity == Vector3.zero)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (isSprinting && CanSlide())
            {
                stateMachine.ChangeState(player.SlidingState);
            }
            else if  (OnCrouch)
            {
                stateMachine.ChangeState(player.CrouchState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        ControlDrag();
        ControlSpeed();
        if (isGrounded && !onSlope)
        {
            player.RB.AddForce(dir.normalized * playerData.moveSpeed * playerData.movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && onSlope)
        {
            player.RB.AddForce(slopMoveDir.normalized * playerData.moveSpeed * playerData.movementMultiplier, ForceMode.Acceleration);
        }

    }



    void ControlSpeed()
    {
        if (isSprinting&& isGrounded)
        {
            playerData.moveSpeed = Mathf.Lerp(playerData.moveSpeed, playerData.sprintSpeed, playerData.acceleration * Time.deltaTime);

            player.Anim.SetFloat("movement", 1f, 0.1f, Time.deltaTime);
            //Debug.Log(player.RB.velocity.magnitude);
        }
        else
        {
            playerData.moveSpeed = Mathf.Lerp(playerData.moveSpeed, playerData.walkSpeed, playerData.acceleration * Time.deltaTime);
            player.Anim.SetFloat("movement", 0f, 0.1f, Time.deltaTime);
            //Debug.Log(player.RB.velocity.magnitude);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            player.RB.drag = playerData.groundDrag;
        }
    }








    #region old
/*
            if (isSprinting)
        {
            player.RB.velocity = (player.orientation.forward* input.y* playerData.sprintMultiplier) + (player.orientation.right* input.x* playerData.sprintMultiplier) + (player.orientation.up* player.RB.velocity.y);
            //player.SetVelocityTest(playerData.sprintMultiplier * dir);
            //player.RB.AddForce(GetMovementVector(-player.RB.velocity, dir.normalized, playerData.sprintMultiplier * Time.fixedDeltaTime) * playerData.moveMultiplier);

            player.Anim.SetFloat("movement", 1f,0.1f, Time.deltaTime);
            Debug.Log(player.RB.velocity.magnitude);
        }
        else if (onSlope && isGrounded)
{
    player.SetVelocityTest(playerData.moveMultiplier * slopMoveDir);

}
else
{
    player.RB.velocity = (player.orientation.forward * input.y * playerData.moveMultiplier) + (player.orientation.right * input.x * playerData.moveMultiplier) + (player.orientation.up * player.RB.velocity.y);

    player.RB.AddForce(player.orientation.transform.forward * input.y * playerData.moveSpeed * Time.deltaTime * 1 * playerData.moveMultiplier);
    player.RB.AddForce(player.orientation.transform.right * input.x * playerData.moveSpeed * Time.deltaTime * playerData.moveMultiplier);
    //player.SetVelocityTest(playerData.moveMultiplier * dir);
    //player.RB.AddForce(GetMovementVector(-player.RB.velocity, dir.normalized, playerData.moveMultiplier * Time.fixedDeltaTime));
    player.Anim.SetFloat("movement", 0f, 0.1f, Time.deltaTime);
    Debug.Log(player.RB.velocity.magnitude);
}


//-----------------------------------------------------------------
public Vector3 GetMovementVector(Vector3 velocity, Vector3 dir, float speed)
{
    if (!isGrounded && velocity.magnitude != 0 && playerData.enableInAirDrag || velocity.magnitude != 0 && playerData.enableInAirDrag && jumpInput)
    {
        float drop = playerData.airDrag * Time.fixedDeltaTime;
        velocity *= drop != 0f ? drop : 1f;

        return new Vector3(velocity.x, 0f, velocity.z) + dir * speed;
    }

    if (isGrounded && velocity.magnitude != 0f && OnCrouch && player.currentSlope >= playerData.maxSlope || isGrounded && Sliding)
    {
        velocity *= playerData.slideFriction * Time.fixedDeltaTime;
        return velocity + dir * speed;
    }

    if (isGrounded && velocity.magnitude != 0f) velocity *= playerData.friction * Time.fixedDeltaTime;
    return velocity + dir * speed;
}

public Vector2 FindVelRelativeToLook()
{
    float current = player.orientation.transform.eulerAngles.y;
    float target = Mathf.Atan2(player.RB.velocity.x, player.RB.velocity.z) * 57.29578f;
    float num = Mathf.DeltaAngle(current, target);
    float num2 = 90f - num;
    float magnitude = new Vector2(player.RB.velocity.x, player.RB.velocity.z).magnitude;
    float num3 = magnitude * Mathf.Cos(num * 0.017453292f);
    return new Vector2(magnitude * Mathf.Cos(num2 * 0.017453292f), num3);
}
*/
    /*private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (x == 0f && y == 0f && player.RB.velocity.magnitude < 1f && isgrounded && !this.jumping && this.playerStatus.CanJump())
        {
            player.RB.isKinematic = true;
        }
        else
        {
            player.RB.isKinematic = false;
        }
        if (!this.grounded || (this.jumping && this.playerStatus.CanJump()))
        {
            return;
        }
        if (this.crouching)
        {
            player.RB.AddForce(this.moveSpeed * 0.02f * -player.RB.velocity.normalized * this.slideCounterMovement);
            return;
        }
        if (Math.Abs(mag.x) > this.threshold && Math.Abs(x) < 0.05f && this.readyToCounterX > 1)
        {
            player.RB.AddForce(this.moveSpeed * this.orientation.transform.right * 0.02f * -mag.x * this.counterMovement);
        }
        if (Math.Abs(mag.y) > this.threshold && Math.Abs(y) < 0.05f && this.readyToCounterY > 1)
        {
            player.RB.AddForce(this.moveSpeed * this.orientation.transform.forward * 0.02f * -mag.y * this.counterMovement);
        }
        if (this.IsHoldingAgainstHorizontalVel(mag))
        {
            player.RB.AddForce(this.moveSpeed * this.orientation.transform.right * 0.02f * -mag.x * this.counterMovement * 2f);
        }
        if (this.IsHoldingAgainstVerticalVel(mag))
        {
            player.RB.AddForce(this.moveSpeed * this.orientation.transform.forward * 0.02f * -mag.y * this.counterMovement * 2f);
        }
        if (Mathf.Sqrt(Mathf.Pow(player.RB.velocity.x, 2f) + Mathf.Pow(player.RB.velocity.z, 2f)) > this.maxSpeed * PowerupInventory.Instance.GetSpeedMultiplier(null))
        {
            float num = player.RB.velocity.y;
            Vector3 vector = player.RB.velocity.normalized * this.maxSpeed * PowerupInventory.Instance.GetSpeedMultiplier(null);
            player.RB.velocity = new Vector3(vector.x, num, vector.z);
        }
        if (Math.Abs(x) < 0.05f)
        {
            this.readyToCounterX++;
        }
        else
        {
            this.readyToCounterX = 0;
        }
        if (Math.Abs(y) < 0.05f)
        {
            this.readyToCounterY++;
            return;
        }
        this.readyToCounterY = 0;
    }

    private void RampMovement(Vector2 mag)
    {
        if (this.grounded && this.onRamp && !this.surfing && !this.crouching && !this.jumping && this.resetJumpCounter >= this.jumpCounterResetTime && Math.Abs(this.x) < 0.05f && Math.Abs(this.y) < 0.05f && !this.pushed)
        {
            player.RB.useGravity = false;
            if (player.RB.velocity.y > 0f)
            {
                player.RB.velocity = new Vector3(player.RB.velocity.x, 0f, player.RB.velocity.z);
                return;
            }
            if (player.RB.velocity.y <= 0f && Mathf.Abs(mag.magnitude) < 1f)
            {
                player.RB.velocity = Vector3.zero;
                return;
            }
        }
        else
        {
            player.RB.useGravity = true;
        }
    }
*/

#endregion



}
