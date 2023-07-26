using UnityEngine;

public class ScorchtailTailAttackState : ScorchtailBaseState
{
    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        enemy.animator.Play("Scorchtail_Tail_Attack");
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {

    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        
    }
}
