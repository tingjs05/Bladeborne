using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchtailPatrolState : ScorchtailBaseState
{
    private bool targetDetected = false;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        // subscribe to target detected event
        enemy.movement.targetDetected += onTargetDetected;

        // set animation to idle
        enemy.animator.Play("Scorchtail_Idle");

        // flip sprite if needed
        enemy.sprite.flipX = enemy.flipSprite;
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

            // check for obstacles in that direction using ray cast
            Vector2 obstacleLocation = enemy.obstacleInDirection((randomPoint - (Vector2) enemy.transform.position).normalized, Vector2.Distance(enemy.transform.position, randomPoint));
            // if an obstacle is detected, travel to the closest possible point in that direction
            if (obstacleLocation != Vector2.zero)
            {
                randomPoint = obstacleLocation;
            } 

            // set new target positions to chase
            enemy.movement.setOverrideTargetPosition(new List<Vector2>() {randomPoint});
        }

        // if taget is detected (either from patrol or detected player), switch to chase state to chase target
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

    // event handlers
    private void onTargetDetected()
    {   
        // set target detected boolean to true when target is detected
        targetDetected = true;
    }
}
