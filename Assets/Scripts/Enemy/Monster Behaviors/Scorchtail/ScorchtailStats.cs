using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchtailStats : MonoBehaviour
{
    [field: Header("Movement")]
    [field: SerializeField] public float walkSpeed {get; private set;} = 1.2f;
    [field: SerializeField] public float runSpeed {get; private set;} = 2.5f;
}
