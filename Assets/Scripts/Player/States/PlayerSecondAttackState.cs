using UnityEngine;

public class PlayerSecondAttackState : PlayerBaseState
{
    private float maxStateDuration = 0.4f;
    private float durationInState;
    private bool backToIdle = false;
    private bool moving = false;

    public override void OnEnter(PlayerController player)
    {
        // play attack animation
        player.weaponAnimator.Play("Weapon_Attack2");

        // set the duration in this state to 0
        durationInState = 0f;
    }

    public override void OnUpdate(PlayerController player)
    {
        // update input
        player.updateInput(false);
        // move player if there is input
        if (player.input != new Vector2(0, 0))
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
        if (player.stamina < player.maxStamina)
        {
            // increase stamina every second
            player.stamina += player.staminaGainPerSecond * Time.deltaTime;
        }

        // check for chain attack
        if (Input.GetKeyDown(player.attackKey) && durationInState < maxStateDuration)
        {
            player.switchState(player.attack3);
        }
        // idle in state
        else if (durationInState < maxStateDuration)
        {
            // increment duration in state
            durationInState += Time.deltaTime;
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
