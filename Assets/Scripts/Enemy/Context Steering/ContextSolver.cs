using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContextSolver : MonoBehaviour
{
    [SerializeField] private SeekBehavior seekBehavior;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private bool showGizmos = true;

    private float[] interestGizmo = new float[8];
    private Vector2 resultDirection = Vector2.zero;
    private float rayLength = 1.2f;

    // additional steering preference behaviors
    private int[] preferredDirection;
    private bool seeking = false;

    void Start()
    {
        seekBehavior.seekState += seekState;
    }

    public Vector2 GetDirectionToMove(List<SteeringBehavior> behaviors, AIData data)
    {
        float[] danger = new float[8];
        float[] interest = new float[8];

        // loop through each behavior and get steering
        foreach (SteeringBehavior behavior in behaviors)
        {
            (danger, interest) = behavior.GetSteering(danger, interest, data);
        }

        // subtract danger values from interest array, so that the AI have less interest to travel to danger directions
        for (int i = 0; i < interest.Length; i++)
        {
            // clamp the value between 0 and 1
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        // if there are no interests and have not reached target location, try to go around obstacle
        if (interest.All(weight => weight == 0) && seeking)
        {
            interest = avoidObstaacles(danger, interest);
        }
        // if there are interests, reset preferred direction back to null
        else if (preferredDirection != null)
        {
            preferredDirection = null;
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

    private float[] avoidObstaacles(float[] danger, float[] interest)
    {
        for (int i = 0; i < (int) interest.Length; i++)
        {
            // check if direction has danger
            if (danger[i] != 0)
            {
                // create preferred direction array if it is null
                if (preferredDirection == null)
                {
                    preferredDirection = new int[8];
                }

                // get the 90 degree direction index from obstacle to try to go to the side of the obstacle
                int directionIndex1 = calculateCircleArrayIndex(i, interest.Length, 2);
                int directionIndex2 = calculateCircleArrayIndex(i, interest.Length, 2, false);

                // subtract danger from interest in that direction
                interest[directionIndex1] = Mathf.Clamp01(1f - danger[directionIndex1]);
                interest[directionIndex2] = Mathf.Clamp01(1f - danger[directionIndex2]);

                // since both directions are opposing, choose one direction out of the two
                interest = chooseDirection(interest, directionIndex1, directionIndex2, i);
            }
        }

        return interest;
    }

    // function for adding and subtracting index in a looped array
    private int calculateCircleArrayIndex(int index, int arrayLength, int repeatAmount, bool add = true)
    {
        // add or subtract index repeatAmount number of times
        for (int i = 0; i < repeatAmount; i++)
        {
            if (add)
            {
                index++;
                // if index exceeds array length, circle it back to the first index of the array
                if (index > arrayLength - 1)
                {
                    index = 0;
                }
            }
            else
            {
                index--;
                // if index is less than first index, circle it to the last index of the array
                if (index < 0)
                {
                    index = arrayLength - 1;
                }
            }
        }

        return index;
    }

    private float[] chooseDirection(float[] interest, int index1, int index2, int directionIndex)
    {
        // get vector2 of corresponding direction
        Vector2 direction1 = Directions.directions[index1];
        Vector2 direction2 = Directions.directions[index2];

        // raycast in both direction
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, direction1, Mathf.Infinity, obstacleLayerMask);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, direction2, Mathf.Infinity, obstacleLayerMask);

        // get the point where the ray collides with an obstacle
        Vector2 point1 = hit1.point;
        Vector2 point2 = hit2.point;

        // get the distance of the nearest obstacle in both directions
        float distance1 = Vector2.Distance(transform.position, point1);
        float distance2 = Vector2.Distance(transform.position, point2);

        // integer to store choice
        int choice;

        // choose to go in the direction with an obstacle further away
        if (distance1 > distance2 || preferredDirection[directionIndex] == 1)
        {
            choice = 1;
        }
        else if (distance2 > distance1 || preferredDirection[directionIndex] == 2)
        {
            choice = 2;
        }
        // if both distances are the same, pick a random one
        else
        {
            System.Random rand = new System.Random();
            choice = rand.Next(1, 3);
        }

        // set direction and cache choice to move in that direction until target can be located
        if (choice == 1)
        {
            preferredDirection[directionIndex] = 1;
            interest[index1] = 0f;
        }
        else
        {
            preferredDirection[directionIndex] = 2;
            interest[index2] = 0f;
        }

        return interest;
    }

    private void OnDrawGizmos() 
    {
        if (Application.isPlaying && showGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, resultDirection * rayLength);

            Gizmos.color = Color.green;
            for (int i = 0; i < interestGizmo.Length; i++)
            {
                Gizmos.DrawRay(transform.position, Directions.directions[i] * interestGizmo[i]);
            }
        }
    }

    // event handlers
    private void seekState(bool isSeeking)
    {
        seeking = isSeeking;
    }
}
