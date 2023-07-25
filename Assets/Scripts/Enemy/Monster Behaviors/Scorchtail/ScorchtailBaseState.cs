using UnityEngine;

public abstract class ScorchtailBaseState : MonoBehaviour
{
    public abstract void OnEnter(ScorchtailStateMachine enemy);

    public abstract void OnUpdate(ScorchtailStateMachine enemy);

    public abstract void OnExit(ScorchtailStateMachine enemy);
}
