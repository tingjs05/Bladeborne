using UnityEngine;

public class ScorchtailWalkState : ScorchtailBaseState
{
    private Vector2 direction = Vector2.zero;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        enemy.animator.Play("Scorchtail_Walk");

        ScorchtailAI.moveDirectionUpdate += updateDirection;
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // move in the direction
        enemy.rb.velocity = enemy.stats.walkSpeed * direction;

        // flip sprite according to direction
        enemy.sprite.flipX = direction.x > 0;
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        
    }

    private void updateDirection(Vector2 calculatedDirection)
    {
        direction = calculatedDirection;
    }
}
