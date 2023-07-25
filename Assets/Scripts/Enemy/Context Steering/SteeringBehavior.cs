using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior : MonoBehaviour
{
    // returns an annonymous tuple of danger weights and intrest weights
    public abstract (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData data);
}
