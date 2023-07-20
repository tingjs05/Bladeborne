using UnityEngine;

public class PlayerFirstAttackState : PlayerBaseState
{
    private float stateDuration = 0.4f;
    private float durationInState;

    private bool bufferedAttack = false;
    private bool backToIdle = false;
    private bool moving = false;

    private Vector2[] attackRange = new[]{
        new Vector2(0f, -0.35f),
        new Vector2(1.2f, -0.35f),
        new Vector2(1.2f, 0.75f),
        new Vector2(0f, 0.4f)
    };

    public override void OnEnter(PlayerController player)
    {
        // play attack animation
        player.weaponAnimator.Play("Weapon_Attack1");

        // set player animation to idle
        player.playerAnimator.Play("Player_Idle");

        // set the duration in this state to 0
        durationInState = 0f;

        // set attack range
        player.setAttackRange(attackRange);

        // activate attack
        player.attackRange.SetActive(true);
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
            player.switchState(player.attack2);
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
}
