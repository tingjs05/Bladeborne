using UnityEngine;

public class ScorchtailChaseState : ScorchtailBaseState
{
    float durationInState;
    float flipThreshold = 0.15f;
    float moveSpeed = 0f;
    float targetDistance = 0f;
    bool targetReached = false;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        // subscribe to target reached event
        enemy.movement.targetReached += onTargetReached;

        // decide whether to walk or run
        decideWalkRun(enemy);

        // set duration in state to 0
        durationInState = 0f;
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // if the target has been reached, return to idle state
        if (targetReached)
        {
            enemy.switchState(enemy.idle);
        }
        // if target has not been reached, but theres no direction, that means target si within range, but has been lost, so switch to patrol state
        // or duration in state is more than patrol delay (bad patrol target and got stuck)
        else if (enemy.moveDirection == Vector2.zero || durationInState >= enemy.patrolDelay)
        {
            enemy.switchState(enemy.patrol);
        }

        // move in the direction
        enemy.rb.velocity = moveSpeed * enemy.moveDirection;

        // flip sprite according to direction
        enemy.sprite.flipX = enemy.moveDirection.x > flipThreshold;

        // update duration in state
        durationInState += Time.deltaTime;
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        // reset override target position on exit since target has been reached
        enemy.movement.setOverrideTargetPosition();

        // reset target reached boolean to false
        targetReached = false;
    }

    private void decideWalkRun(ScorchtailStateMachine enemy)
    {
        // get distance between enemy and target
        targetDistance = Vector2.Distance(enemy.transform.position, enemy.movement.getData().currentTarget);

        // if target is furthur than minRunDistance, run towards the target
        if (targetDistance >= enemy.minRunDistance)
        {
            moveSpeed = enemy.stats.runSpeed;
            enemy.animator.Play("Scorchtail_Run");
        }
        // else if the target is closer than minRunDistance, walk towards the target
        else
        {
            moveSpeed = enemy.stats.walkSpeed;
            enemy.animator.Play("Scorchtail_Walk");
        }
    }

    // event handlers
    private void onTargetReached()
    {
        // set target reached boolean to true when target is reached
        targetReached = true;
    }
}
