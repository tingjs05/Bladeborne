using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused {get; private set;} = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private float pauseCooldown = 0.2f;
    private bool canPause = true;
    private bool cooldown = false;
    private float cooldownCounter = 0f;
    private KeyCode pauseKey = KeyCode.Escape;

    // Update is called once per frame
    void Update()
    {
        // have a cooldown when allowed to pause (canPause set to true)
        if (cooldown)
        {
            // increment cooldown counter
            cooldownCounter += Time.deltaTime;

            if (cooldownCounter >= pauseCooldown)
            {
                // reset cooldown when cooldown is over
                cooldown = false;
                cooldownCounter = 0f;
            }
        }

        // if pause key is pressed, toggle pause menu
        if (Input.GetKeyDown(pauseKey) && canPause && !cooldown)
        {
            togglePause();
        }
    }

    public void togglePause()
    {
        // if not paused, pause the game
        if (!isPaused)
        {
            // show pause menu
            pauseMenu.SetActive(true);
            // freeze game by setting time scale to 0
            Time.timeScale = 0f;
            // set is paused variable
            isPaused = true;
            return;
        }
        
        // unfreeze game by setting time scale to 1
        Time.timeScale = 1f;
        // set is paused variable
        isPaused = false;
        // hide pause menu
        pauseMenu.SetActive(false);
        // start pause cooldown so pause cannot be spammed
        cooldown = true;
    }

    public void disablePause(bool disable)
    {
        // if pause is disabled, cannot pause
        canPause = !disable;

        // if cannot pause, unpause the game if paused
        if (!canPause && isPaused)
        {
            togglePause();
        }
        else if (canPause)
        {
            // start can pause cooldown
            cooldown = true;
        }
    }

    // on click methods
    public void returnFromLevel(int sceneIndex)
    {   
        // unpause the game before switching scenes if game is paused
        if (isPaused)
        {
            togglePause();
        }

        // switch scene to scene to return to
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        // log that we have quit the game
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
