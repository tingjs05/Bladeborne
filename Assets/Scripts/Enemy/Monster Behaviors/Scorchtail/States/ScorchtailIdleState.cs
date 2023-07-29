using UnityEngine;

public class ScorchtailIdleState : ScorchtailBaseState
{
    bool targetDetected = false;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        // subscribe to target detected event
        enemy.movement.targetDetected += onTargetDetected;

        enemy.animator.Play("Scorchtail_Idle");
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // if player is within attack range, attack the player
        if (playersInAttackRange(enemy.transform.position, enemy.attackRange, enemy.playerLayerMask))
        {
            enemy.switchState(enemy.scratchAtk);
        }

        // if taget is detected, switch to chase state to chase target
        if (targetDetected)
        {
            enemy.switchState(enemy.chase);
        }
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        // reset target detected boolean to false
        targetDetected = false;
    }

    // detect if players are within attack range
    public bool playersInAttackRange(Vector2 position, float attackRange, LayerMask playerLayerMask)
    {
        // detect players within range
        Collider2D[] players = Physics2D.OverlapCircleAll(position, attackRange, playerLayerMask);

        // if any player is detected, return true
        if (players != null && players?.Length > 0)
        {
            return true;
        }

        // else return false
        return false;
    }

    // event handlers
    private void onTargetDetected()
    {   
        // set target detected boolean to true when target is detected
        targetDetected = true;
    }
}
