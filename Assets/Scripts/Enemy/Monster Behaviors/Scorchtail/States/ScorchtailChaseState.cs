using UnityEngine;

public class ScorchtailChaseState : ScorchtailBaseState
{
    float flipThreshold = 0.15f;
    float moveSpeed = 0f;
    float targetDistance = 0f;
    Vector2 targetLocation = Vector2.zero;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        Debug.Log("Chase");
        // decide whether to walk or run
        decideWalkRun(enemy);
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // if there are targets in range and is not seeking target, that means target is lost, so start patrolling
        if (enemy.movement.getData().currentTarget == Vector2.zero)
        {
            enemy.switchState(enemy.patrol);
        }

        // move in the direction
        enemy.rb.velocity = moveSpeed * enemy.moveDirection;

        // flip sprite according to direction
        enemy.sprite.flipX = enemy.moveDirection.x > flipThreshold;
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        enemy.movement.setOverrideTargetPosition();
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
}
