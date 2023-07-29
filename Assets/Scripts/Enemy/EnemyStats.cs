using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStats : MonoBehaviour
{
    public abstract void setHealth(float value);

    public abstract void changeHealth(float value);
}
