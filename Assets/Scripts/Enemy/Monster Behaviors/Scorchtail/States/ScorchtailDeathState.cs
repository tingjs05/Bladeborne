using UnityEngine;

public class ScorchtailDeathState : ScorchtailBaseState
{
    private float disappearSpeed = 1.5f;
    private bool paused = false;

    public override void OnEnter(ScorchtailStateMachine enemy)
    {
        paused = false;
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
            Time.timeScale = 0f;
            paused = true;
        }
    }

    public override void OnExit(ScorchtailStateMachine enemy)
    {

    }
}
