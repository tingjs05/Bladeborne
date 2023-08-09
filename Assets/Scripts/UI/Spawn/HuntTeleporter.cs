using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntTeleporter : MonoBehaviour
{
    [SerializeField] private HuntMenu huntMenu;

    void OnTriggerEnter2D(Collider2D collider)
    {
        // check of collider is a player
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            // play teleporter activate sound
            GetComponent<SoundEffects>().playSound("Activate");
            // open menu when player steps in
            huntMenu.openMenu(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        // check of collider is a player
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            // play teleporter deactivate sound
            GetComponent<SoundEffects>().playSound("Deactivate");
            // close menu when player steps in
            huntMenu.openMenu(false);
        }
    }
}
