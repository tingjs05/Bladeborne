using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchtailStats : MonoBehaviour
{
    [field: Header("Health")]
    [field: SerializeField] public float maxHealth {get; private set;} = 1000.0f;
    private float health;
    // create a public Health reference for health, and clamp health to keep value within range
    public float Health
    {
        get
        {
            return health;
        } 
        set
        {
            health = Mathf.Clamp(value, 0f, maxHealth);
        }
    }

    [field: Header("Movement")]
    [field: SerializeField] public float walkSpeed {get; private set;} = 1.2f;
    [field: SerializeField] public float runSpeed {get; private set;} = 2.5f;

    [field: Header("Scratch Attack")]
    [field: SerializeField] public float scratchAttackDamage {get; private set;} = 50.0f;
    [field: SerializeField] public float scratchAttackRange {get; private set;} = 0.75f;

    [field: Header("Tail Whip Attack")]
    [field: SerializeField] public float tailAttackDamage {get; private set;} = 100.0f;
    [field: SerializeField] public float tailAttackRange {get; private set;} = 0.75f;

    [field: Header("Roll Attack")]
    [field: SerializeField] public float rollAttackDamage {get; private set;} = 250.0f;
    [field: SerializeField] public float rollAttackRange {get; private set;} = 0.8f;
    [field: SerializeField] public float rollAttackSpeed {get; private set;} = 3.0f;
}
