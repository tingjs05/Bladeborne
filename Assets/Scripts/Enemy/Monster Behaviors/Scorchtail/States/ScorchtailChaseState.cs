using UnityEngine;

public class ScorchtailChaseState : ScorchtailBaseState
{
    private float flipThreshold = 0.15f;
    private float moveSpeed = 0f;
    private float targetDistance = 0f;
    private bool targetReached = false;

    // unstuck variables
    private float durationInState;
    private float maxStuckDuration = 0.5f;
    private float minObstacleDistance = 2.0f;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        // subscribe to target reached event
        enemy.movement.targetReached += onTargetReached;

        // decide whether to walk or run depending on the distance from target
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
            // ensure it doesnt run the code below when need to switch state
            return;
        }
        // if target has not been reached, but theres no direction, that means target si within range, but has been lost, so switch to patrol state
        // or if there are obstacles in the way and more than max state duration (meaning the enemy got stuck), so it would switch to patrol state to try to unstuck itself
        else if (enemy.moveDirection == Vector2.zero || (enemy.obstacleInDirection(enemy.moveDirection, minObstacleDistance) != Vector2.zero && durationInState >= maxStuckDuration))
        {
            enemy.switchState(enemy.patrol);
            // ensure it doesnt run the code below when need to switch state
            return;
        }

        // decide whether to walk or run depending on the distance from target
        decideWalkRun(enemy);

        // move in the direction
        enemy.rb.velocity = moveSpeed * enemy.moveDirection;

        // flip sprite according to direction
        enemy.sprite.flipX = enemy.moveDirection.x > flipThreshold;

        // cache whether sprite is flipped
        enemy.flipSprite = enemy.moveDirection.x > flipThreshold;

        // increment duration in state
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
