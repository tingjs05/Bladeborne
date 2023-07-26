using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBehavior : SteeringBehavior
{
    // radius is the obstacle detection radius, and colliderSize is half of the colldier size of the enemy (min distance)
    [SerializeField] private float radius = 2.0f, colliderSize = 0.6f;

    [SerializeField] private bool showGizmos = true;

    private float[] dangersResultTemp;

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
            for (int i = 0; i < Directions.directions.Length; i++)
            {
                // calculate dot product of these two vectors, giving an estimate of how close is the direction to the direction to move in
                float result = Vector2.Dot(directionToObstacleNormalized, Directions.directions[i]);

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
                Gizmos.DrawRay(transform.position, Directions.directions[i] * dangersResultTemp[i]);
            }
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
