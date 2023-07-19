using UnityEngine;

public class PlayerSecondAttackState : PlayerBaseState
{
    private Vector2 attackOffset = new Vector2(0.6f, 0.55f);
    private float stateDuration = 0.4f;
    private float durationInState;
    private bool bufferedAttack = false;
    private bool backToIdle = false;
    private bool moving = false;

    public override void OnEnter(PlayerController player)
    {
        // play attack animation
        player.weaponAnimator.Play("Weapon_Attack2");

        // set the duration in this state to 0
        durationInState = 0f;

        // get attack point, weapon pos + attack offset and multiply by direction of attack
        // Vector2 attackPoint =  player.mouseDirection * (attackOffset * player.weapon.transform.position);

        // // detect enemies in range of attack
        // Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint, player.attackRange2);

        // foreach (Collider2D enemy in enemies)
        // {
        //     Debug.Log(enemy.name);
        //     // damage the enemy if hit
        //     if (enemy.gameObject.tag == "Enemy")
        //     {
        //         Debug.Log("Attack 2 hit an Enemy!");
        //     }
        // }
    }

    public override void OnUpdate(PlayerController player)
    {
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
        // switch weapon animation back to idle if player is returning to idle state
        if (backToIdle)
        {
            player.weaponAnimator.Play("Weapon_Idle");
            backToIdle = false;
        }
    }
}
