using UnityEngine;

public class ScorchtailScratchAttackState : ScorchtailBaseState
{
    private float durationInState;
    private float attackDelay = 0.55f;
    private float maxStateDuration = 0.85f;
    private bool attacked;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        // set duration in state to 0
        durationInState = 0f;

        // set attacked to false
        attacked = false;

        enemy.animator.Play("Scorchtail_Scratch_Attack");

        // flip sprite if needed
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

        // detect players in range when ready to attack
        if (durationInState >= attackDelay && !attacked)
        {
            attacked = true;

            // attack player
            enemy.attack(enemy.transform.position, enemy.stats.scratchAttackRange, enemy.stats.scratchAttackDamage);

            // play attack sound
            enemy.sound.playSound("Scratch Attack");
        }

        // switch back to idle state when attack is done
        if (durationInState >= maxStateDuration)
        {
            enemy.switchState(enemy.idle);
        }
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        
    }
}
