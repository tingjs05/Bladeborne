using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchtailPatrolState : ScorchtailBaseState
{
    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        enemy.animator.Play("Scorchtail_Idle");
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // get random location around the enemy within patrol range
        Vector2 randomPoint = Random.insideUnitCircle * enemy.patrolRange;

        // set new target positions
        enemy.movement.setOverrideTargetPosition(new List<Vector2>() {randomPoint});
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {

    }
}
