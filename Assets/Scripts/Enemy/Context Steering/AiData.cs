using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    [HideInInspector] public List<Vector2> targets = null;
    [HideInInspector] public List<Vector2> obstacles = null;

    [HideInInspector] public Vector2 currentTarget;
}
