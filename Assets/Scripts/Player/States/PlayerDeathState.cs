using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    private float disappearSpeed = 1.5f;
    private bool paused = false;

    public override void OnEnter(PlayerController player)
    {
        paused = false;
    }

    public override void OnUpdate(PlayerController player)
    {
        // gradually make sprite disappear when dead
        if (player.playerRenderer.color.a > 0f)
        {
            player.playerRenderer.color = new Color(player.playerRenderer.color.r, player.playerRenderer.color.g, player.playerRenderer.color.b, 
                player.playerRenderer.color.a - (disappearSpeed * Time.deltaTime));
            player.weaponRenderer.color = new Color(player.weaponRenderer.color.r, player.weaponRenderer.color.g, player.weaponRenderer.color.b, 
                player.weaponRenderer.color.a - (disappearSpeed * Time.deltaTime));
        }
        // pause the game once player has completely disappeared, only pause it once, therefore the paused boolean
        else if (!paused)
        {
            Time.timeScale = 0f;
            paused = true;
        }
    }

    public override void OnExit(PlayerController player)
    {

    }
}
