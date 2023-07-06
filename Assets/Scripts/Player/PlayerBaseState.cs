using UnityEngine;

public abstract class PlayerBaseState : MonoBehaviour
{
    public abstract void OnEnter(PlayerController player);

    public abstract void OnUpdate(PlayerController player);

    public abstract void OnExit(PlayerController player);
}
