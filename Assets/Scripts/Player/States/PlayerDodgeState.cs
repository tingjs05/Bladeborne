using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    private float currentDodgeSpeed;
    private Vector2 dodgeDirection;

    public override void OnEnter(PlayerController player)
    {   
        // play dodge animation
        player.playerAnimator.Play("Player_Dodge");

        // update input so that can get input
        player.updateInput();
        // update position variables
        player.updateCurrentPos();
        player.updateMousePos();

        // follow mouse direction
        if (player.input == new Vector2(0, 0))
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
        player.stamina -= player.dodgeStaminaCost;
    }

    public override void OnUpdate(PlayerController player)
    {
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

    }
}
