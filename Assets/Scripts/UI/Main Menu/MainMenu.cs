using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LoadingScreen loadingScreen;

    public void PlayGame()
    {
        // play click sound
        GetComponent<SoundEffects>().playSound("Select");
        // load game scene
        loadingScreen.LoadScene(1);
    }

    public void QuitGame()
    {
        // play click sound
        GetComponent<SoundEffects>().playSound("Select");
        // log that we have quit the game
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
