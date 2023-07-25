using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBehavior : SteeringBehavior
{
    // radius is the obstacle detection radius, and colliderSize is half of the colldier size of the enemy (min distance)
    [SerializeField] private float radius = 2.0f, colliderSize = 0.6f;

    [SerializeField] private bool showGizmos = true;

    private List<Vector2> directions = new List<Vector2>();
    private float[] dangersResultTemp;

    // Start is called before the first frame update
    void Start()
    {
        // set the list contatining 8 directions
        getEightDirections();
    }

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData data)
    {
        foreach (Vector2 obstaclePosition in data.obstacles)
        {
            // get obstacle direction and distance to obstacle
            Vector2 directionToObstacle = obstaclePosition - (Vector2) transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;

            float weight;

            // calculate weight based on the distance between the enemy and the obstacle
            if (distanceToObstacle <= colliderSize)
            {
                // if obstacle is closer than colliderSize (min distance), avoid that direction at all costs
                weight = 1.0f;
            }
            else
            {
                // calculate weight based on detection radius
                weight = (radius - distanceToObstacle) / radius;
            }

            Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

            // add obstacle parameters to danger array
            for (int i = 0; i < directions.Count; i++)
            {
                // calculate dot product of these two vectors, giving an estimate of how close is the direction to the direction to move in
                float result = Vector2.Dot(directionToObstacleNormalized, directions[i]);

                float valueToPutIn = result * weight;

                // overried value only if it is higher than the current one stored in the array
                if (valueToPutIn > danger[i])
                {
                    danger[i] = valueToPutIn;
                }
            }
        }
        dangersResultTemp = danger;
        return (danger, interest);
    }

    private void getEightDirections()
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
                directions.Add(direction.normalized);
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

        if (Application.isPlaying && dangersResultTemp != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < dangersResultTemp.Length; i++)
            {
                Gizmos.DrawRay(transform.position, directions[i] * dangersResultTemp[i]);
            }
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
