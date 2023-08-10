using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    private float disappearSpeed = 1.5f;
    private bool paused = false;
    private Transform deathParticles;

    public override void OnEnter(PlayerController player)
    {
        paused = false;

        // stop all sounds
        player.sound.stopSound();

        // set deathParticles to null
        deathParticles = null;

        // instatiate death particles and store transform
        deathParticles = Instantiate(player.deathParticles, player.transform.position, Quaternion.identity).transform;

        // set player as parent
        deathParticles.SetParent(player.transform);
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
            // destroy death particles
            Destroy(deathParticles.gameObject);
            // completely pause game after death
            Time.timeScale = 0f;
            paused = true;
        }
    }

    public override void OnExit(PlayerController player)
    {

    }
}
