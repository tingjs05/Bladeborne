using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField] private float targetDetectionRange = 5.0f;
    [SerializeField] private LayerMask obstacleLayerMask, playerLayerMask;
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private bool showRaycast = true;

    // gizmos parameters
    private List<Vector2> colliderPositions;

    public override void Detect(AIData data)
    {
        // find out if player is near, only detect one player collider
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayerMask);

        // if the player is detected within range
        if (playerCollider != null)
        {
            // check if you see the player
            Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
            // use raycast to check line of sight
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, targetDetectionRange, obstacleLayerMask);

            // check if the player can be seen, and is on the player layer or player invulnerable layer
            if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                // show raycast if showRaycast is true
                if (showRaycast)
                {
                    Debug.DrawRay(transform.position, direction * targetDetectionRange, Color.magenta);
                }

                // set colliderPositions list to detected player collider
                colliderPositions = new List<Vector2>() {playerCollider.transform.position};
            }
            else
            {
                // set colliderPositions list if no player is detected
                colliderPositions = null;
            }

            // store target position data
            data.targets = colliderPositions;
        }
    }

    private void OnDrawGizmos()
    {
        // do not show gizmos if showGizmos is false
        if (!showGizmos)
        {
            return;
        }

        // show target detection range with wire sphere gizmos
        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        // only show detected player if a player is detected
        if (colliderPositions == null)
        {
            return;
        }

        Gizmos.color = Color.magenta;
        foreach (Vector2 position in colliderPositions)
        {
            Gizmos.DrawSphere(position, 0.1f);
        }
    }
}
