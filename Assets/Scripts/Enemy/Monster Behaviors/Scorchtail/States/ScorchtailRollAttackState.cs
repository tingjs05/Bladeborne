using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScorchtailRollAttackState : ScorchtailBaseState
{
    private float healthOnEnter;
    private Vector2 target;
    private Vector2 direction;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        // set target to (0, 0) by default
        target = Vector2.zero;

        // get target position
        getTarget(enemy.transform.position, enemy.rollAttackActivationRange, enemy.playerLayerMask);

        // if there are no targets, return to idle state
        if (target == Vector2.zero)
        {
            enemy.switchState(enemy.idle);
            return;
        }

        // set target direction
        direction = (target - (Vector2) enemy.transform.position).normalized;

        // detect obstacles in that direction
        Vector2 obstacleLocation = enemy.obstacleInDirection(direction, Vector2.Distance(enemy.transform.position, target));
        // if there are obstacles in that direction, change target to the obstacle location
        if (obstacleLocation != Vector2.zero)
        {
            target = obstacleLocation;
        }

        // cache health when just entered state
        healthOnEnter = enemy.stats.health;

        enemy.animator.Play("Scorchtail_Roll_Attack");
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // update sprite flip
        enemy.sprite.flipX = direction.x > enemy.flipThreshold;

        // set flip sprite boolean
        enemy.flipSprite = direction.x > enemy.flipThreshold;

        // if health is lower than first entered state (the enemy got hit) and player has not been hit, enter stunned state
        if (enemy.stats.health < healthOnEnter)
        {
            enemy.switchState(enemy.stun);
        }

        // move towards target
        enemy.rb.velocity = enemy.stats.rollAttackSpeed * direction;

        // if reached within attack range, switch back to idle
        if (Vector2.Distance(enemy.transform.position, target) <= enemy.attackRange)
        {
            enemy.switchState(enemy.idle);
            return;
        }

        // attack player
        enemy.attack(enemy.transform.position, enemy.stats.rollAttackRange, enemy.stats.rollAttackDamage * Time.deltaTime);
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {
        // deal damage on exit
        enemy.attack(enemy.transform.position, enemy.stats.rollAttackRange, enemy.stats.rollAttackDamage);
    }

    // find the players in range
    public void getTarget(Vector2 position, float range, LayerMask playerLayerMask)
    {
        // detect players within range
        Collider2D[] players = Physics2D.OverlapCircleAll(position, range, playerLayerMask);

        target = players.OrderBy(collider => Vector2.Distance(collider.transform.position, position)).Last().transform.position;
    }
}
