using UnityEngine;

public class PlayerThirdAttackState : PlayerBaseState
{
    private float stateDuration = 0.7f;
    private float durationInState;
    private float attackDelay = 0.3f;

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
    }

    public override void OnUpdate(PlayerController player)
    {
        // activate attack after delay
        if (durationInState >= attackDelay)
        {
            // activate attack
            player.attackRange.SetActive(true);
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
        // deactivate attack
        player.attackRange.SetActive(false);

        // reset adjusted weapon position
        player.weaponSprite.transform.Translate(new Vector3(0f, 0.05f, 0f));

        // switch weapon animation back to idle
        player.weaponAnimator.Play("Weapon_Idle");
    }
}
