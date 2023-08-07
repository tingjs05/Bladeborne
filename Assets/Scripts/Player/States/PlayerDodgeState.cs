using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    private float currentDodgeSpeed;
    private Vector2 dodgeDirection;
    private bool sheathedToDodge = false;

    public override void OnEnter(PlayerController player)
    {
        // play dodge animation
        player.playerAnimator.Play("Player_Dodge");

        // play dodge sound
        player.sound.playSound("Player Dodge");

        // set player layer to ignore damage
        player.gameObject.layer = LayerMask.NameToLayer("Player Invulnerable");

        //check if player's weapon is sheathed, otherwise sheath weapon
        if (!player.isSheathed)
        {
            player.toggleSheath();
            sheathedToDodge = true;
        }

        // update input so that can get input
        player.updateInput();
        // update position variables
        player.updateCurrentPos();
        player.updateMousePos();

        // follow mouse direction
        if (player.input == Vector2.zero)
        {
            // get direction of mouse from player
            dodgeDirection = -(player.currentPos - player.mouseWorldPos).normalized;
        }
        // follow input direction
        else
        {
            dodgeDirection = player.input;
        }

        // set current dodge speed
        currentDodgeSpeed = player.dodgeSpeed;

        // reduce stamina
        player.Stamina -= player.dodgeStaminaCost;
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

        // continue dodging
        player.rb.velocity = dodgeDirection * currentDodgeSpeed;
        if (currentDodgeSpeed >= player.minDodgeSpeed)
        {
            currentDodgeSpeed -= player.dodgeSpeedDropOffScale * Time.deltaTime;
        }
        else
        {
            // change state to idle when finished dodging
            player.switchState(player.idle);
        }
    }

    public override void OnExit(PlayerController player)
    {
        // unsheath weapon if it is sheathed to run
        if (sheathedToDodge)
        {
            player.toggleSheath();
            sheathedToDodge = false;
        }

        // set player layer to default player layer
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
