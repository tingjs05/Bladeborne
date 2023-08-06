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
            // open menu when player steps in
            huntMenu.openMenu(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        // check of collider is a player
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            // close menu when player steps in
            huntMenu.openMenu(false);
        }
    }
}
