using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    private float minSprintStamina = 10.0f;

    public override void OnEnter(PlayerController player)
    {
        // play walk animation
        player.playerAnimator.Play("Player_Walk");
    }

    public override void OnUpdate(PlayerController player)
    {
        // only increase stamina when below max
        if (player.stamina < player.maxStamina)
        {
            // increase stamina every second
            player.stamina += player.staminaGainPerSecond * Time.deltaTime;
        }

        // prioritize checking for dodge
        if (Input.GetKeyDown(player.dodgeKey) && player.stamina >= player.dodgeStaminaCost)
        {
            player.switchState(player.dodge);
        }
        // sprint if sprintKey is pressed and required stamina is met
        else if (Input.GetKey(player.sprintKey) && player.stamina >= minSprintStamina)
        {
            player.switchState(player.run);
        }
        // continue walking if still want to walk
        else if (player.input != new Vector2(0, 0))
        {
            // move player
            player.rb.velocity = player.input * player.walkSpeed;
        }
        // switch state to idle if other conditions are not met
        else
        {
            player.switchState(player.idle);
        }
    }

    public override void OnExit(PlayerController player)
    {

    }
}
