using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThirdAttackState : PlayerBaseState
{
    private float stateDuration = 0.7f;
    private float durationInState;
    private float attackDelay = 0.3f;
    private float attackForce = 15.0f;

    private bool attacked;

    private GameObject enemy;
    private List<GameObject> enemiesHit;

    private Vector2[] attackRange = new[]{
        new Vector2(-0.1f, -0.35f),
        new Vector2(1.75f, -0.15f),
        new Vector2(1.75f, 0.2f),
        new Vector2(-0.1f, 0.35f)
    };

    public override void OnEnter(PlayerController player)
    {
        // play attack animation
        player.weaponAnimator.Play("Weapon_Attack3");

        // set player animation to walk
        player.playerAnimator.Play("Player_Walk");

        // set the duration in this state to 0
        durationInState = 0f;

        // adjust weapon position
        player.weaponSprite.transform.Translate(new Vector3(0f, -0.05f, 0f));

        // set attack range
        player.setAttackRange(attackRange);

        // set attacked to false
        attacked = false;

        // subscribe to enemy hit event
        player.attackDetection.enemyHit += dealDamage;

        // reset enemies hit list
        enemiesHit = new List<GameObject>();

        // reset enemy to null
        enemy = null;
    }

    public override void OnUpdate(PlayerController player)
    {
        // check if the player is still alive
        if (player.Health <= 0f)
        {
            player.switchState(player.death);
        }

        // deal damage if an enemy is hit and has not been hit before
        if (enemy != null && !enemiesHit.Contains(enemy))
        {
            enemy.GetComponent<EnemyStats>().changeHealth(-player.attackDamage3);

            // create damage number popup
            player.damageEffects.Create(enemy.transform.position, Mathf.RoundToInt(player.attackDamage3));

            // instantiate hit particles and set player as parent
            Instantiate(player.hitParticles, enemy.transform.position, Quaternion.identity).transform.SetParent(player.transform);

            // log the enemies that have been hit
            enemiesHit.Add(enemy);
        }
    
        // activate attack after delay
        if (durationInState >= attackDelay && !attacked)
        {
            // activate attack
            player.attackRange.SetActive(true);

            // play attack sound
            player.sound.playSound("Player Attack 3");

            // move player a little in the direction of the attack by adding a impulse force
            player.rb.AddForce(player.mouseDirection * attackForce, ForceMode2D.Impulse);

            // set attacked to true
            attacked = true;
        }

        // only increase stamina when below max
        if (player.Stamina < player.maxStamina)
        {
            // increase stamina every second
            player.Stamina += player.staminaGainPerSecond * Time.deltaTime;
        }

        if (durationInState < stateDuration)
        {
            // increment duration in state
            durationInState += Time.deltaTime;
        }
        // if at max state duration, switch back to idle
        else
        {
            player.switchState(player.idle);
        }
    }

    public override void OnExit(PlayerController player)
    {
        // deactivate attack
        player.attackRange.SetActive(false);

        // reset adjusted weapon position
        player.weaponSprite.transform.Translate(new Vector3(0f, 0.05f, 0f));

        // switch weapon animation back to idle
        player.weaponAnimator.Play("Weapon_Idle");
    }

    // event handlers
    private void dealDamage(GameObject enemy)
    {
        this.enemy = enemy;
    }
}
