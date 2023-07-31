using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSecondAttackState : PlayerBaseState
{
    private float stateDuration = 0.4f;
    private float durationInState;

    private bool bufferedAttack = false;
    private bool backToIdle = false;
    private bool moving = false;

    private GameObject enemy;
    private List<GameObject> enemiesHit;

    private Vector2[] attackRange = new[]{
        new Vector2(0.2f, -0.3f),
        new Vector2(1.35f, -0.3f),
        new Vector2(1.35f, 0.6f),
        new Vector2(-0.3f, 1.0f)
    };

    public override void OnEnter(PlayerController player)
    {
        // play attack animation
        player.weaponAnimator.Play("Weapon_Attack2");

        // set the duration in this state to 0
        durationInState = 0f;

        // set attack range
        player.setAttackRange(attackRange);

        // activate attack
        player.attackRange.SetActive(true);

        // subscribe to enemy hit event
        player.attackDetection.enemyHit += dealDamage;

        // reset enemies hit list
        enemiesHit = new List<GameObject>();

        // reset enemy to null
        enemy = null;
    }

    public override void OnUpdate(PlayerController player)
    {
        // deal damage if an enemy is hit and has not been hit before
        if (enemy != null && !enemiesHit.Contains(enemy))
        {
            enemy.GetComponent<EnemyStats>().changeHealth(-player.attackDamage2);

            // create damage number popup
            player.damageEffects.Create(enemy.transform.position, Mathf.RoundToInt(player.attackDamage2));

            // log the enemies that have been hit
            enemiesHit.Add(enemy);
        }

        // update input
        player.updateInput(false);
        // move player if there is input
        if (player.input != Vector2.zero)
        {
            // move player
            player.rb.velocity = player.input * player.walkSpeed;
            // set animation to walk
            player.playerAnimator.Play("Player_Walk");
            // set moving boolean
            moving = true;
        }
        // if moving and no input, reset to idle
        else if (moving)
        {
            // reset animation to idle
            player.playerAnimator.Play("Player_Idle");
            // reset moving boolean
            moving = false;
        }

        // only increase stamina when below max
        if (player.Stamina < player.maxStamina)
        {
            // increase stamina every second
            player.Stamina += player.staminaGainPerSecond * Time.deltaTime;
        }

        // check for chain attack
        if (Input.GetKeyDown(player.attackKey) && durationInState < stateDuration)
        {
            // allow players to buffer their next attack
            bufferedAttack = true;
        }
        // idle in state
        else if (durationInState < stateDuration)
        {
            // increment duration in state
            durationInState += Time.deltaTime;
        }
        // change to next attack when at max state duration if player clicked during state duration
        else if (bufferedAttack && durationInState >= stateDuration)
        {
            // reset buffered attack boolean
            bufferedAttack = false;
            // switch to next attack state
            player.switchState(player.attack3);
        }
        // if at max state duration, switch back to idle
        else
        {
            backToIdle = true;
            player.switchState(player.idle);
        }
    }

    public override void OnExit(PlayerController player)
    {
        // deactivate attack
        player.attackRange.SetActive(false);

        // switch weapon animation back to idle if player is returning to idle state
        if (backToIdle)
        {
            player.weaponAnimator.Play("Weapon_Idle");
            backToIdle = false;
        }
    }

    // event handlers
    private void dealDamage(GameObject enemy)
    {
        this.enemy = enemy;
    }
}
