using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void OnEnter(PlayerController player)
    {
        // set velocity to 0 when in idle state
        player.rb.velocity = Vector2.zero;

        // play idle animation
        player.playerAnimator.Play("Player_Idle");
    }

    public override void OnUpdate(PlayerController player)
    {
        // update input
        player.updateInput();

        // check if the player is still alive
        if (player.Health <= 0f)
        {
            player.switchState(player.death);
        }

        // check if the player wants to attack
        if (Input.GetKeyDown(player.attackKey))
        {
            // unsheath weapon if sheathed
            if (player.isSheathed)
            {
                player.toggleSheath();
                // aim weapon to set mouse position
                player.aimWeapon();
            }
            player.switchState(player.attack1);
        }

        // check if the player wants to sheath weapon
        if (Input.GetKeyDown(player.sheathKey))
        {
            player.toggleSheath();
        }
        // update weapon rotation if weapon is not sheathed
        if (!player.isSheathed)
        {
            player.aimWeapon();
        }

        // only increase stamina when below max
        if (player.Stamina < player.maxStamina)
        {
            // increase stamina every second
            player.Stamina += player.staminaGainPerSecond * Time.deltaTime;
        }

        // prioritize checking for dodge
        if (Input.GetKeyDown(player.dodgeKey) && player.Stamina >= player.dodgeStaminaCost)
        {
            player.switchState(player.dodge);
        }
        // check for movement
        else if (player.input != Vector2.zero)
        {
            // if there are inputs, switch state to walk
            player.switchState(player.walk);
        }
    }

    public override void OnExit(PlayerController player)
    {

    }
}
