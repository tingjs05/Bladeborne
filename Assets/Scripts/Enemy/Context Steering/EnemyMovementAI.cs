using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyMovementAI : MonoBehaviour
{
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private List<SteeringBehavior> steeringBehaviors;
    [SerializeField] private ContextSolver movementDirectionSolver;
    [SerializeField] private AIData data;
    [SerializeField] private float detectionDelay = 0.05f;

    // enemy AI events
    public event Action targetDetected;
    public event Action targetReached;

    // override target event
    public event Action<List<Vector2>> overrideTarget;

    // Start is called before the first frame update
    void Start()
    {
        // detect player and obstacle at set intervals instead of every frame
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    // create a public method to callback the GetDirectionToMove() method
    public Vector2 getDirectionToMove() => movementDirectionSolver.GetDirectionToMove(steeringBehaviors, data);

    // create a public method to override target behavior
    public void setOverrideTargetPosition(List<Vector2> targets = null) { overrideTarget?.Invoke(targets); }

    // get AI data
    public AIData getData() => data;

    // detect player and obstacles
    private void PerformDetection()
    {
        // locate nearby targets and obstacles
        foreach (Detector detector in detectors)
        {
            detector.Detect(data);
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkEvents();
    }

    private void checkEvents()
    {
        if (data.targets != null || data.targets?.Count > 0)
        {
            // raise event if reached target (no current target) and target is detected within range
            if (data.currentTarget == Vector2.zero)
            {
                targetReached?.Invoke();
            }
            // raise an event if targets are detected, and not reached
            else
            {
                targetDetected?.Invoke();
            }
        }
    }    
}
