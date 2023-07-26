using UnityEngine;

public class ScorchtailWalkState : ScorchtailBaseState
{

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        enemy.animator.Play("Scorchtail_Walk");
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // move in the direction
        enemy.rb.velocity = enemy.stats.walkSpeed * enemy.moveDirection;

        // flip sprite according to direction
        enemy.sprite.flipX = enemy.moveDirection.x > 0;
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        
    }
}
