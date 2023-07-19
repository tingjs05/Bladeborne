using UnityEngine;

public class PlayerThirdAttackState : PlayerBaseState
{
    private Vector2 attackOffset = new Vector2(0.8f, 0.4f);
    private float stateDuration = 0.7f;
    private float durationInState;

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

        // get attack point, weapon pos + attack offset and multiply by direction of attack
        // Vector2 attackPoint = player.mouseDirection * (attackOffset * player.weapon.transform.position);

        // // detect enemies in range of attack
        // Collider2D[] enemies = Physics2D.OverlapBoxAll(attackPoint, player.attackRange3, 0f);

        // foreach (Collider2D enemy in enemies)
        // {
        //     Debug.Log(enemy.name);
        //     // damage the enemy if hit
        //     if (enemy.gameObject.tag == "Enemy")
        //     {
        //         Debug.Log("Attack 3 hit an Enemy!");
        //     }
        // }
    }

    public override void OnUpdate(PlayerController player)
    {
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

            // move player a little in the direction of the attack
            player.rb.velocity = player.mouseDirection * 0.25f;
        }
        // if at max state duration, switch back to idle
        else
        {
            player.switchState(player.idle);
        }
    }

    public override void OnExit(PlayerController player)
    {
        // reset adjusted weapon position
        player.weaponSprite.transform.Translate(new Vector3(0f, 0.05f, 0f));

        // switch weapon animation back to idle
        player.weaponAnimator.Play("Weapon_Idle");
    }
}
