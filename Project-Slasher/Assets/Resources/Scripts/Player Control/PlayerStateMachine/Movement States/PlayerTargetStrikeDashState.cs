using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetStrikeDashState : PlayerDashState
{
    public PlayerTargetStrikeDashState(PlayerStateMachine context, PlayerStateFactory factory) : base(context,factory) { }

    public override void EnterState()
    {
        base.EnterState();
        Context.colliderSwitcher.SwitchToCollider(4);
        Vector3 dashDirection = (Context.combatTarget.transform.position - Context.playerCenter.position).normalized;
        // Weight the saved velocity less if it's in the wrong direction
        cachedSpeed *= Mathf.Sqrt((Vector3.Dot(Context.playerRb.velocity.normalized, dashDirection) + 1f) / 2f);
        Context.playerPhysicsTransform.forward = dashDirection;
        Context.forwardVector = dashDirection;
        Context.playerRb.velocity = dashDirection * Context.combatProfile.HitVelocity;
    }

    public override void ExitState()
    {
        base.ExitState();
        Context.playerRb.velocity = Context.playerRb.velocity.normalized * (cachedSpeed + Context.combatProfile.HitBoostVelocity);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void CheckSwitchState()
    {
            
    }
}
