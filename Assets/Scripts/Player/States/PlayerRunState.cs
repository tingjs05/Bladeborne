using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    private float staminaSecondCount = 0.0f;

    public override void OnEnter(PlayerController player)
    {
        
    }

    public override void OnUpdate(PlayerController player)
    {
        // prioritize checking for dodge
        if (Input.GetKey(player.dodgeKey) && player.stamina >= player.dodgeStaminaCost)
        {
            player.switchState(player.dodge);
        }
        // continue sprinting if sprintKey is pressed and stamina is not 0
        else if (Input.GetKey(player.sprintKey) && player.stamina >= 0)
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

    }
}
