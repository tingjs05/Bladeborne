using UnityEngine;

public class ScorchtailDeathState : ScorchtailBaseState
{
    private float disappearSpeed = 1.5f;
    private bool paused = false;
    private Transform deathParticles;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        paused = false;

        // stop all sounds
        enemy.sound.stopSound();

        // set deathParticles to null
        deathParticles = null;

        // instatiate death particles and store transform
        deathParticles = Instantiate(enemy.deathParticles, enemy.transform.position, Quaternion.identity).transform;

        // set enemy as parent
        deathParticles.SetParent(enemy.transform);
    }

    public override void OnUpdate(ScorchtailStateMachine enemy)
    {
        // gradually make sprite disappear when dead
        if (enemy.sprite.color.a > 0f)
        {
            enemy.sprite.color = new Color(enemy.sprite.color.r, enemy.sprite.color.g, enemy.sprite.color.b, 
                enemy.sprite.color.a - (disappearSpeed * Time.deltaTime));
        }
        // pause the game once enemy has completely disappeared, only pause it once, therefore the paused boolean
        else if (!paused)
        {
            // destroy death particles
            Destroy(deathParticles.gameObject);
            // completely pause game after death
            Time.timeScale = 0f;
            paused = true;
        }
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {

    }
}
