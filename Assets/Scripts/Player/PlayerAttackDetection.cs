using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackDetection : MonoBehaviour
{
    public event System.Action<GameObject> enemyHit;

    void OnTriggerEnter2D(Collider2D collider)
    {
        manageCollision(collider);
    }

    void manageCollision(Collider2D collider)
    {
        // only detect enemy isTrigger collider
        if (collider.CompareTag("Enemy") && collider.isTrigger)
        {
            enemyHit?.Invoke(collider.gameObject);
        }
    }
}
