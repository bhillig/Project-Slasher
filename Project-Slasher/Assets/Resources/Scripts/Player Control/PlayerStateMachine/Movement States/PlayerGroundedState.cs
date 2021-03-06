using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerStateMachine context, PlayerStateFactory factory) : base(context,factory) {}

    protected float groundedJumpDelayTimer;

    public override void EnterState()
    {
        base.EnterState();
        Context.inputContext.JumpDownEvent.AddListener(Jump);
        Context.inputContext.SlideDownEvent.AddListener(Shift);
    }

    public override void ExitState()
    {
        base.ExitState();
        Context.inputContext.JumpDownEvent.RemoveListener(Jump);
        Context.inputContext.SlideDownEvent.RemoveListener(Shift);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        groundedJumpDelayTimer -= Time.deltaTime;
    }
    public override void FixedUpdateState()
    {

    }

    public override void CheckSwitchState()
    {
        if (!Context.groundPhysicsContext.IsGrounded())
        {
            TrySwitchState(Factory.Jump);
        }
    }
    protected virtual void Jump()
    {
        if (groundedJumpDelayTimer <= 0)
        {
            groundedJumpDelayTimer = Context.movementProfile.GroundedToJumpDelay;
            TrySwitchState(Factory.Jump);
            // Jump physics
            Vector3 vel = Context.playerRb.velocity;
            vel.y = Mathf.Max(0f, vel.y);
            Vector3 jumpVec = Context.groundPhysicsContext.ContactNormal * Context.movementProfile.JumpVelocity;
            vel += jumpVec;
            Context.playerRb.velocity = vel;
            Context.groundPhysicsContext.GroundedBlockTimer = Context.movementProfile.JumpGroundBlockDuration;
            Context.audioManager.jumpEmitter.Play();
            // Start particles.
            Context.Particle = GameObject.Instantiate(Context.JumpParticle, Context.transform, false);

        }
    }

    protected virtual void Shift()
    {
        TrySwitchState(Factory.Slide);
    }
}
