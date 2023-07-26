using UnityEngine;

public class ScorchtailRollAttackState : ScorchtailBaseState
{
    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        enemy.animator.Play("Scorchtail_Roll_Attack");
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {

    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        
    }
}
