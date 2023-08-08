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

        // stop sound effects
        enemy.sound.stopSound();

        // flip sprite if needed
        enemy.sprite.flipX = enemy.flipSprite;

        durationSinceLastAttack = 0f;
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // check if enemy is still alive
        if (enemy.stats.getHealth() <= 0f)
        {
            enemy.switchState(enemy.death);
        }

        // if player is within attack range and duration since last attack is more than attac cooldown (or there is no last attack), attack the player
        if (enemy.playersInRange(enemy.attackRange) && (durationSinceLastAttack >= enemy.attackCooldown || !attacked))
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
                // ensure only one state change option is chosen
                return;
            }
            // higher chance to use scratch attack
            else
            {
                enemy.switchState(enemy.scratchAtk);
                // ensure only one state change option is chosen
                return;
            }
        }

        // check if player is within range for a roll attack, and not within min range
        if (enemy.playersInRange(enemy.rollAttackActivationRange) && !enemy.playersInRange(enemy.rollAttackMinRange))
        {
            // random chance to activate roll attack
            System.Random rand = new System.Random();
            int choice = rand.Next(0, 3);

            if (choice == 0)
            {
                enemy.switchState(enemy.rollAtk);
                // ensure only one state change option is chosen
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

    // event handlers
    private void onTargetDetected()
    {   
        // set target detected boolean to true when target is detected
        targetDetected = true;
    }
}
