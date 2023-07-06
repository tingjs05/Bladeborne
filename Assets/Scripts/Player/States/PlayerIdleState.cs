using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void OnEnter(PlayerController player)
    {
        // set velocity to 0 when in idle state
        player.rb.velocity = new Vector2(0, 0);
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
        if (Input.GetKey(player.dodgeKey) && player.stamina >= player.dodgeStaminaCost)
        {
            player.switchState(player.dodge);
        }
        // check for movement
        else if (player.input != new Vector2(0, 0))
        {
            // if there are inputs, switch state to walk
            player.switchState(player.walk);
        }
    }

    public override void OnExit(PlayerController player)
    {

    }
}
