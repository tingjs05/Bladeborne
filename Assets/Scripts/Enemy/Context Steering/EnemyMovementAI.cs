using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementAI : MonoBehaviour
{
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private List<SteeringBehavior> steeringBehaviors;
    [SerializeField] private AIData data;
    [SerializeField] private float detectionDelay = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        // detect player and obstacle at set intervals instead of every frame
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    // detect player and obstacles
    private void PerformDetection()
    {
        // locate nearby targets and obstacles
        foreach (Detector detector in detectors)
        {
            detector.Detect(data);
        }

        // create arrays danger and interest weights of 8 directions
        float[] danger = new float[8];
        float[] interest = new float[8];

        foreach (SteeringBehavior behavior in steeringBehaviors)
        {
            // set danger and interest arrays to the output of GetSteering()
            (danger, interest) = behavior.GetSteering(danger, interest, data);
        }
    }
}
