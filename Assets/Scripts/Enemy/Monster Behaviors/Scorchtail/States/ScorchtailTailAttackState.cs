using UnityEngine;

public class ScorchtailTailAttackState : ScorchtailBaseState
{
    private float durationInState;
    private float attackDelay = 0.65f;
    private float maxStateDuration = 1.05f;
    private float attackOffset = -0.5f;
    private bool attacked;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        // set duration in state to 0
        durationInState = 0f;

        // set attacked to false
        attacked = false;

        enemy.animator.Play("Scorchtail_Tail_Attack");

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

            Vector2 attackPosition;

            // flip the attack offset if the sprite is flipped
            if (!enemy.sprite.flipX)
            {
                attackPosition = new Vector2(enemy.transform.position.x + attackOffset, enemy.transform.position.y);
            }
            else
            {
                attackPosition = new Vector2(enemy.transform.position.x - attackOffset, enemy.transform.position.y);
            }

            // attack player
            enemy.attack(attackPosition, enemy.stats.tailAttackRange, enemy.stats.tailAttackDamage);

            // play attack sound
            enemy.sound.playSound("Tail Attack");
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
