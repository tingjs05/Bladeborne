using UnityEngine;

public class ScorchtailIdleState : ScorchtailBaseState
{
    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        enemy.animator.Play("Scorchtail_Idle");
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {

    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        
    }
}
