using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementAI : MonoBehaviour
{
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private List<SteeringBehavior> steeringBehaviors;
    [SerializeField] private ContextSolver movementDirectionSolver;
    [SerializeField] private AIData data;
    [SerializeField] private float detectionDelay = 0.05f;

    // override target event
    public static event System.Action<List<Vector2>> overrideTarget;

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

    // detect player and obstacles
    private void PerformDetection()
    {
        // locate nearby targets and obstacles
        foreach (Detector detector in detectors)
        {
            detector.Detect(data);
        }
    }
}
