using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    [HideInInspector] public List<Vector2> targets = null;
    [HideInInspector] public List<Vector2> obstacles = null;

    [HideInInspector] public Vector2 currentTarget;

    // return 0 if targets is null, else return length of targets list to prevent null exception
    public int GetTargetsCount()
    {
        if (targets == null)
        {
            return 0;
        }
        else
        {
            return targets.Count;
        }
    }
}
