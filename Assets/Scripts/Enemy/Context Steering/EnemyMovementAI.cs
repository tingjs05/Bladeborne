using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementAI : MonoBehaviour
{
    [SerializeField] private List<Detector> detectors;
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
        foreach (Detector detector in detectors)
        {
            detector.Detect(data);
        }
    }
}
