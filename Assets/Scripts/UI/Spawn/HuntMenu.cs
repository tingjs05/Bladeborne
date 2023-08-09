using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private LoadingScreen loadingScreen;
    [SerializeField] private GameObject[] otherUIElements;
    private bool isOpen = false;
    private KeyCode exitKey = KeyCode.Escape;
    private SoundEffects sound;
    private Coroutine sceneSwitch;

    // Start is called before the first frame update
    void Start()
    {
        // get sound effects component
        sound = GetComponent<SoundEffects>();
    }

    // Update is called once per frame
    void Update()
    {
        // close the menu when the exit key is pressed and the menu is open
        if (Input.GetKeyDown(exitKey) && isOpen)
        {
            openMenu(false);
        }
    }

    // go to selected level
    public void goToLevel(int levelIndex)
    {
        // only switch scene if not already switching scene
        if (sceneSwitch != null)
        {
            return;
        }

        // play teleport sound
        sound.playSound("Teleport");
        // switch scene to level
        sceneSwitch = StartCoroutine(delaySceneChange(levelIndex, 2.0f));
    }

    // method to open/close menu
    public void openMenu(bool open)
    {
        // only open menu if menu is not null
        if (menu == null)
        {
            return;
        }

        // open the menu by setting the menu game object to active if want to open menu
        if (open)
        {
            // play open menu sound
            sound.playSound("Open");
            // show menu
            menu.SetActive(open);
            // make sure the user cannot pause when hunt menu is open
            pauseMenu.disablePause(open);
            // hide other ui elements when opening menu
            setUI(!open);
            // set isOpen boolean
            isOpen = true;
            return;
        }

        // play close menu sound
        sound.playSound("Close");
        // show other ui elements when closing menu
        setUI(!open);
        // set isOpen boolean
        isOpen = false;
        // allow the user to pause the game since the hunt menu is now closed
        pauseMenu.disablePause(open);
        // else set the menu game object to not active if want to close menu
        menu.SetActive(open);
    }

    // set other ui elements
    private void setUI(bool open)
    {
        foreach (GameObject element in otherUIElements)
        {
            element.SetActive(open);
        }
    }

    // wait a while before changing scene
    private IEnumerator delaySceneChange(int levelIndex, float delay)
    {
        float elaspedTime = 0f;

        // increment elaspedTime
        while (elaspedTime < delay)
        {
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        // load the scene
        loadingScreen.LoadScene(levelIndex);
    }
}
