using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallglideState : PlayerMovementState
{
    public PlayerWallglideState(PlayerStateMachine context, PlayerStateFactory factory) : base(context,factory)
    {
        
    }

    public override void EnterState()
    {
        Context.playerRb.useGravity = false;
        Context.animationController.SetBool("Airborne", true);
        Context.inputContext.JumpDownEvent.AddListener(OnSpacebarDown);
    }

    public override void ExitState()
    {
        Context.playerRb.useGravity = true;
        Context.animationController.SetBool("Airborne", false);
        Context.wallRunning.isWallRunning = false;
        Context.inputContext.JumpDownEvent.RemoveListener(OnSpacebarDown);
    }

    public void OnSpacebarDown()
    {
        Context.wallRunning.JumpFromWall();
        TrySwitchState(Factory.Jump);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Context.wallRunning.DetectWalls();
        Context.wallRunning.CheckDuration();
    }

    public override void FixedUpdateState()
    {

    }

    public override void CheckSwitchState()
    {
        if (!Context.wallRunning.IsWallRunning())
        {
            TrySwitchState(Factory.Jump);
            return;
        }

        //Grounded check
        if (Context.groundPhysicsContext.IsGrounded())
        {
            TrySwitchState(Factory.Idle);
        }
    }
}
