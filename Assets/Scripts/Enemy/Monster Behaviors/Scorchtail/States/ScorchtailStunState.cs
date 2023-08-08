using UnityEngine;

public class ScorchtailStunState : ScorchtailBaseState
{
    private float durationInState;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        enemy.animator.Play("Scorchtail_Stun");

        // play sound
        enemy.sound.playSound("Stun");

        // reset duration in state to 0
        durationInState = 0f;

        // update sprite flip
        enemy.sprite.flipX = enemy.flipSprite;
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // check if enemy is still alive
        if (enemy.stats.getHealth() <= 0f)
        {
            enemy.switchState(enemy.death);
        }

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
