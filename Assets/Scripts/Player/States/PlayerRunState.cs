using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    private float staminaSecondCount = 0.0f;
    private bool sheathedToRun = false;

    public override void OnEnter(PlayerController player)
    {
        // play run animation
        player.playerAnimator.Play("Player_Run");

        //check if player's weapon is sheathed, otherwise sheath weapon
        if (!player.isSheathed)
        {
            player.toggleSheath();
            sheathedToRun = true;
        }
    }

    public override void OnUpdate(PlayerController player)
    {
        // update input
        player.updateInput();

        // prioritize checking for dodge
        if (Input.GetKeyDown(player.dodgeKey) && player.stamina >= player.dodgeStaminaCost)
        {
            player.switchState(player.dodge);
        }
        // continue sprinting if sprintKey is pressed and stamina is not 0 and there is movement input
        else if (Input.GetKey(player.sprintKey) && player.stamina >= 0 && player.input != new Vector2(0, 0))
        {
            player.rb.velocity = player.input * player.sprintSpeed;

            // reduce stamina when sprinting
            staminaSecondCount += Time.deltaTime;
            if (staminaSecondCount >= player.staminaDrainPerSecond)
            {
                player.stamina -= 1;
                staminaSecondCount = 0.0f;
            }
        }
        // walk if cannot sprint but still want to move
        else if (player.input != new Vector2(0, 0))
        {
            player.switchState(player.walk);
        }
        // switch to idle if other conditions are not met
        else
        {
            player.switchState(player.idle);
        }
    }

    public override void OnExit(PlayerController player)
    {
        // unsheath weapon if it is sheathed to run
        if (sheathedToRun)
        {
            player.toggleSheath();
            sheathedToRun = false;
        }
    }
}
