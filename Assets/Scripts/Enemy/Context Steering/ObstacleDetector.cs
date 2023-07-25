using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : Detector
{
    [SerializeField] private float detectionRange = 2.0f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private bool showRaycast = true;

    private List<Vector2> colliderPositions;
    private List<Vector2> directions = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        // set the list contatining 8 directions
        getDirections();
    }

    // detect surrounding obstacles using overlap circle
    public override void Detect(AIData data)
    {
        // reset colliderPositions list
        colliderPositions = new List<Vector2>();
        // raycast in all 8 directions to detect obstacles
        foreach (Vector2 direction in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, layerMask);

            // show raycast if showRaycast is true
            if (showRaycast)
            {
                Debug.DrawRay(transform.position, direction * detectionRange, Color.red);
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

    private void getDirections()
    {
        // array of possible point combinations
        int[] pointsArray = new[] {0, 1, -1};
        // create current direction
        Vector2 direction = Vector2.zero;

        for (int i = 0; i < pointsArray.Length; i++)
        {
            // set x coordinate
            direction.x = pointsArray[i];

            for (int j = 0; j < pointsArray.Length; j++)
            {
                // set y coordinate
                direction.y = pointsArray[j];
                // add direction to directions list
                directions.Add(direction);
            }
        }

        // remove the first coordinate, which is (0, 0)
        directions.RemoveAt(0);
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
