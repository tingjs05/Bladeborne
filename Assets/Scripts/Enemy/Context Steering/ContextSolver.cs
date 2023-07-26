using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSolver : MonoBehaviour
{
    [SerializeField] private bool showGizmos = true;

    private float[] interestGizmo = new float[8];
    private Vector2 resultDirection = Vector2.zero;
    private float rayLength = 1.2f;

    public Vector2 GetDirectionToMove(List<SteeringBehavior> behaviors, AIData data)
    {
        float[] danger = new float[8];
        float[] interest = new float[8];

        // loop through each behavior and get steering
        foreach (SteeringBehavior behavior in behaviors)
        {
            (danger, interest) = behavior.GetSteering(danger, interest, data);
        }

        // substract danger values from interest array, so that the AI have less interest to travel to danger directions
        for (int i = 0; i < interest.Length; i++)
        {
            // clamp the value between 0 and 1
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        interestGizmo = interest;

        // get the average direction
        Vector2 outputDirection = Vector2.zero;
        for (int i = 0; i < interest.Length; i++)
        {
            outputDirection += Directions.directions[i] * interest[i];
        }
        outputDirection.Normalize();

        resultDirection = outputDirection;

        // return selected movement direction
        return resultDirection;
    }

    private void OnDrawGizmos() 
    {
        if (Application.isPlaying && showGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, resultDirection * rayLength);
        }
    }
}
