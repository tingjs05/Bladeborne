using UnityEngine;

public class ScorchtailRunState : ScorchtailBaseState
{
    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        enemy.animator.Play("Scorchtail_Run");
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {

    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        
    }
}
