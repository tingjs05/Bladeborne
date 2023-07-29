using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchtailStats : EnemyStats
{
    [field: Header("Health")]
    [field: SerializeField] public float maxHealth {get; private set;} = 1500.0f;
    public float health {get; private set;}

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
    [field: SerializeField] public float rollAttackDamage {get; private set;} = 150.0f;
    [field: SerializeField] public float rollAttackRange {get; private set;} = 0.8f;
    [field: SerializeField] public float rollAttackSpeed {get; private set;} = 3.0f;

    // public method to set health
    public override void setHealth(float value)
    {
        health = Mathf.Clamp(value, 0f, maxHealth);
    }

    // public method to change health
    public override void changeHealth(float value)
    {
        health = Mathf.Clamp(health + value, 0f, maxHealth);
    }
}
