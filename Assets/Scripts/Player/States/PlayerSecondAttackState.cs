using UnityEngine;

public class PlayerSecondAttackState : PlayerBaseState
{
    private float maxStateDuration = 0.4f;
    private float durationInState;
    private bool backToIdle = false;

    public override void OnEnter(PlayerController player)
    {
        // play attack animation
        player.weaponAnimator.Play("Weapon_Attack2");

        // set the duration in this state to 0
        durationInState = 0f;
    }

    public override void OnUpdate(PlayerController player)
    {
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
