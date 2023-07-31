using UnityEngine;

public class ScorchtailStunState : ScorchtailBaseState
{
    private float durationInState;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        enemy.animator.Play("Scorchtail_Stun");

        // reset duration in state to 0
        durationInState = 0f;

        // update sprite flip
        enemy.sprite.flipX = enemy.flipSprite;
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // increment duration in state
        durationInState += Time.deltaTime;

        // return to idle state after stun duration
        if (durationInState >= enemy.stunDuration)
        {
            enemy.switchState(enemy.idle);
        }
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        
    }
}
