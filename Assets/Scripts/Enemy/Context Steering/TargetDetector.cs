using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField] private float targetDetectionRange = 5.0f;
    [SerializeField] private LayerMask obstacleLayerMask, playerLayerMask;
    [SerializeField] private EnemyMovementAI enemyMovementAI;
    [SerializeField] private Vector2 spawnPos = new Vector2(0f, 0f);
    [SerializeField] private float durationBeforeReturningToSpawn = 5.0f;
    [SerializeField] private bool showGizmos = true;

    private List<Vector2> colliderPositions;
    private List<Vector2> overrideTargetPositions;
    private float durationSinceLostTarget = 0f;
    private bool lostTarget = false;

    void Start()
    {
        // subscribe to overried target event
        enemyMovementAI.overrideTarget += overrideTarget;
    }

    void Update()
    {
        // update durationSinceLostTarget if target is lost (out of range)
        if (lostTarget)
        {
            durationSinceLostTarget += Time.deltaTime;
        }
    }

    public override void Detect(AIData data)
    {
        // reset colliderPositions list to null
        colliderPositions = null;

        // override target detection if there are set target positions to go to
        if (overrideTargetPositions != null && overrideTargetPositions.Count > 0)
        {
            data.targets = overrideTargetPositions;
            return;
        }

        // find out if a player is nearby
        Collider2D[] playerColliders = Physics2D.OverlapCircleAll(transform.position, targetDetectionRange, playerLayerMask);

        // if the player is detected within range
        if (playerColliders != null && playerColliders?.Length > 0)
        {
            // when found target, reset returning to spawn
            lostTarget = false;
            durationSinceLostTarget = 0f;

            // add each player to target list
            foreach (Collider2D playerCollider in playerColliders)
            {
                // check if you see the player
                Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
                // use raycast to check line of sight
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, targetDetectionRange, obstacleLayerMask);

                // check if the player can be seen, and is on the player layer or player invulnerable layer
                if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0 && !playerCollider.isTrigger)
                {
                    // show raycast if showGizmos is true
                    if (showGizmos)
                    {
                        Debug.DrawRay(transform.position, direction * targetDetectionRange, Color.magenta);
                    }

                    // create a new colliderPositions list if list is null
                    if (colliderPositions == null)
                    {
                        colliderPositions = new List<Vector2>();
                    }

                    // add player collider position to colliderPositions list if it doesn't exist
                    if (!colliderPositions.Contains((Vector2) hit.point))
                    {
                        colliderPositions.Add(hit.point);
                    }
                }
            }
        }
        // if there are no players detected within range, go back to spawn position after a certain period
        else if (durationSinceLostTarget > durationBeforeReturningToSpawn && lostTarget)
        {
            colliderPositions = new List<Vector2>() {spawnPos};
        }
        else if (!lostTarget)
        {
            lostTarget = true;
        }
        // store target position data
        data.targets = colliderPositions;
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

    // event handler
    private void overrideTarget(List<Vector2> targets)
    {
        overrideTargetPositions = targets;
    }
}
