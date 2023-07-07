using UnityEngine;

public class PlayerThirdAttackState : PlayerBaseState
{
    private float maxStateDuration = 0.7f;
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
    }

    public override void OnUpdate(PlayerController player)
    {
        if (durationInState < maxStateDuration)
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
