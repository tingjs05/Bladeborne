using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchtailPatrolState : ScorchtailBaseState
{
    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        Debug.Log("Patrolling...");
        // play idle animation in case not changing to chase state to patrol
        enemy.animator.Play("Scorchtail_Idle");
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // only set a new target if duration since last patrol is more than the patrol delay
        if (enemy.durationSinceLastPatrol >= enemy.patrolDelay)
        {
            // reset patrol duration counter when entering patrol
            enemy.resetPatrolCounter();

            // get random location around the enemy within patrol range
            Vector2 randomPoint = (Vector2) enemy.transform.position + Random.insideUnitCircle * enemy.patrolRange;

            // set new target positions to chase
            enemy.movement.setOverrideTargetPosition(new List<Vector2>() {randomPoint});
        }
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {

    }
}
