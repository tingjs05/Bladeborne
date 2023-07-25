using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackDetection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        manageCollision(collider);
    }

    void manageCollision(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && collider.GetType() == typeof(CapsuleCollider2D))
        {
            Debug.Log("Enemy Hit!");
        }
    }
}
