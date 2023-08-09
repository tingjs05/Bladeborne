using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private LoadingScreen loadingScreen;
    [SerializeField] private PlayerController[] players;
    [SerializeField] private EnemyStats[] enemies;
    private SoundEffects sound;
    private bool isOpen;

    public event System.Action<bool> hasWon;

    // Start is called before the first frame update
    void Start()
    {
        // get sound effects component
        sound = GetComponent<SoundEffects>();
        // reset win and lose menus
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        // reset the menu is open boolean to false
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        // check for win
        if (checkWin() && !isOpen)
        {
            // only open menu if condition is met, and menu is not open
            openMenu(winMenu);
            // invoke event
            hasWon?.Invoke(true);
        }
        else if (checkLose() && !isOpen)
        {
            // only open menu if condition is met, and menu is not open
            openMenu(loseMenu);
            // invoke event
            hasWon?.Invoke(false);
        }
    }

    // bring up win or lose menu
    private void openMenu(GameObject menu)
    {
        menu.SetActive(true);
        // pause game when bringing up menu
        Time.timeScale = 0.5f;
        // set isOpen to true
        isOpen = true;
        // do not allow player to pause
        if (pauseMenu != null)
        {
            pauseMenu.disablePause(true);
        }
    }

    // check for win or lose
    private bool checkWin()
    {
        // if all enemy's health are less than equal 0, the player has won
        if (enemies.All(enemy => enemy.getHealth() <= 0f))
        {
            return true;
        }

        // else, the player hasn't won yet
        return false;
    }

    private bool checkLose()
    {
        // if all player's health are less than equal 0, the player has lost
        if (players.All(player => player.Health <= 0f))
        {
            return true;
        }

        // else, the player hasn't lost yet
        return false;
    }

    // on click methods
    public void restartLevel()
    {
        // play click sound
        sound.playSound("Select");
        // unpause before changing scenes
        Time.timeScale = 1f;
        // switch to current scene to reset scene
        loadingScreen.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void returnFromLevel(int sceneIndex)
    {   
        // play click sound
        sound.playSound("Select");
        // unpause before changing scenes
        Time.timeScale = 1f;
        // switch scene to scene to return to
        loadingScreen.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        // play click sound
        sound.playSound("Select");
        // log that we have quit the game
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
