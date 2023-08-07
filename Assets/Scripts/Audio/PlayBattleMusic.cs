using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBattleMusic : MonoBehaviour
{
    [SerializeField] private AudioSource battleMusic;
    [SerializeField] private EnemyMovementAI enemyTargetDetector;
    private bool playing = false;

    // Start is called before the first frame update
    void Start()
    {
        // subscribe to target detected event
        enemyTargetDetector.targetDetected += targetDetected;
    }

    private void targetDetected()
    {
        if (!playing)
        {
            // play audio
            battleMusic.Play();
            playing = true;
        }
    }
}
