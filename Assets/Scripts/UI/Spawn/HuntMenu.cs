using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HuntMenu : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private GameObject[] otherUIElements;
    private bool isOpen = false;
    private GameObject menu;
    private KeyCode exitKey = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {
        menu = transform.GetChild(0).gameObject;
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
        // switch scene to level
        SceneManager.LoadScene(levelIndex);
    }

    // method to open/close menu
    public void openMenu(bool open)
    {
        // open the menu by setting the menu game object to active if want to open menu
        if (open)
        {
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
}
