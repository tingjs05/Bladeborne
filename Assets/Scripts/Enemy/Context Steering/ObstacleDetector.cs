using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : Detector
{
    [SerializeField] private float detectionRange = 2.0f;
    [SerializeField] private int raysToShoot = 50;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private bool showGizmos = true;

    private List<Vector2> colliderPositions;

    // detect surrounding obstacles using overlap circle
    public override void Detect(AIData data)
    {
        // reset colliderPositions list to null
        colliderPositions = null;

        // raycast around yourself to detect obstacles
        for (int i = 0; i < raysToShoot; i++)
        {
            // get ray direction
            Vector2 direction = (Quaternion.Euler(0, 0, i * (360 / raysToShoot)) * transform.right).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, layerMask);

            // show raycast if showGizmos is true
            if (showGizmos)
            {
                Debug.DrawRay(transform.position, direction * detectionRange, Color.cyan);
            }

            // create a new colliderPositions list if list is null
            if (colliderPositions == null)
            {
                colliderPositions = new List<Vector2>();
            }

            if (hit.collider != null)
            {
                // add position raycast hit if something is hit
                colliderPositions.Add(hit.point);
            }
        }
        // store obstacle data
        data.obstacles = colliderPositions;
    }

    private void OnDrawGizmos()
    {
        // do not show gizmos if showGizmos is false
        if (!showGizmos)
        {
            return;
        }

        if (Application.isPlaying && colliderPositions != null)
        {
            // set gizmos sphere color
            Gizmos.color = Color.red;
            // draw sphere gizmos on all detected obstacles
            foreach (Vector2 obstaclePosition in colliderPositions)
            {
                Gizmos.DrawSphere(obstaclePosition, 0.1f);
            }
        }
    }
}
