using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekBehavior : SteeringBehavior
{
    [SerializeField] private float targetReachedThreshold = 0.5f;

    [SerializeField] private bool showGizmos = true;

    private bool reachedLastTarget = true;
    private Vector2 targetPositionCached;
    private float[] interestTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData data)
    {
        // if don't have a target, stop seeking, else set a new target
        if (reachedLastTarget)
        {
            if (data.targets == null || data.targets.Count <= 0)
            {
                data.currentTarget = Vector2.zero;
                return (danger, interest);
            }
            else
            {
                reachedLastTarget = false;
                // set target position
                setTarget(data);
            }
        }
        else
        {
            // update target position if haven't reached target
            setTarget(data);
        }

        // cache the last position only if we still see the target (if the targets list is not empty)
        if (data.currentTarget != Vector2.zero && data.targets != null && data.targets.Contains(data.currentTarget))
        {
            // cache player's position to move to player's last seen location if player is hiding behind an object
            targetPositionCached = data.currentTarget;
        }

        // first check if we have reached the target
        if (Vector2.Distance(transform.position, targetPositionCached) < targetReachedThreshold)
        {
            reachedLastTarget = true;
            data.currentTarget = Vector2.zero;
            return (danger, interest);
        }

        // if havent reached target, find interest direction
        Vector2 directionToTarget = targetPositionCached - (Vector2) transform.position;
        for (int i = 0; i < interest.Length; i++)
        {
            // calculate dot product of these two vectors, giving an estimate of how close is the direction to the direction to move in
            float result = Vector2.Dot(directionToTarget.normalized, Directions.directions[i]);

            // accept only directions less than 90 degrees to the target direction
            if (result > 0)
            {
                float valueToPutIn = result;

                // overried value only if it is higher than the current one stored in the array
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }
            }
        }
        interestTemp = interest;
        return(danger, interest);
    }

    private void setTarget(AIData data)
    {
        if (data.targets != null)
        {
            // set current target to closest target in target list if there are targets
            data.currentTarget = data.targets.OrderBy(target => Vector2.Distance(target, transform.position)).First();
        }
        else
        {
            // set target to zero if there are no targets
            data.currentTarget = Vector2.zero;
        }
    }

    private void OnDrawGizmos()
    {
        // do not show gizmos if showGizmos is false
        if (!showGizmos)
        {
            return;
        }

        if (Application.isPlaying && interestTemp != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < interestTemp.Length; i++)
            {
                Gizmos.DrawRay(transform.position, Directions.directions[i] * interestTemp[i]);
            }
            if (reachedLastTarget == false)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(targetPositionCached, 0.1f);
            }
        }
    }
}
