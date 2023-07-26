using UnityEngine;

public class ScorchtailScratchAttackState : ScorchtailBaseState
{
    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        enemy.animator.Play("Scorchtail_Scratch_Attack");
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {

    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        
    }
}
