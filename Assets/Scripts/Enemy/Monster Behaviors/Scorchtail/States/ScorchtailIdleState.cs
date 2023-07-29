using UnityEngine;

public class ScorchtailIdleState : ScorchtailBaseState
{
    private float durationSinceLastAttack;

    private bool targetDetected = false;
    private bool attacked = false;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        // subscribe to target detected event
        enemy.movement.targetDetected += onTargetDetected;

        enemy.animator.Play("Scorchtail_Idle");

        // flip sprite if needed
        enemy.sprite.flipX = enemy.flipSprite;

        durationSinceLastAttack = 0f;
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // if player is within attack range and duration since last attack is more than attac cooldown (or there is no last attack), attack the player
        if (playersInAttackRange(enemy.transform.position, enemy.attackRange, enemy.playerLayerMask) && (durationSinceLastAttack >= enemy.attackCooldown || !attacked))
        {
            // randomly choose betwwen tail whip or scratch attack
            System.Random rand = new System.Random();
            int choice = rand.Next(0, 3);

            // set attacked to true
            attacked = true;

            // lower chance to use tail whip attack
            if (choice == 0)
            {
                enemy.switchState(enemy.tailAtk);
                return;
            }
            // higher chance to use scratch attack
            else
            {
                enemy.switchState(enemy.scratchAtk);
                return;
            }
        }

        // if taget is detected, switch to chase state to chase target
        if (targetDetected)
        {
            enemy.switchState(enemy.chase);

            // set attacked to false
            attacked = false;
        }

        // increment duration since last attack
        durationSinceLastAttack += Time.deltaTime;
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        // reset target detected boolean to false
        targetDetected = false;
    }

    // detect if players are within attack range
    public bool playersInAttackRange(Vector2 position, float attackRange, LayerMask playerLayerMask)
    {
        // detect players within range
        Collider2D[] players = Physics2D.OverlapCircleAll(position, attackRange, playerLayerMask);

        // if any player is detected, return true
        if (players != null && players?.Length > 0)
        {
            return true;
        }

        // else return false
        return false;
    }

    // event handlers
    private void onTargetDetected()
    {   
        // set target detected boolean to true when target is detected
        targetDetected = true;
    }
}
