using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchtailAI : MonoBehaviour
{
    [SerializeField] private ScorchtailStateMachine stateMachine;
    [SerializeField] private EnemyMovementAI movement;

    // events
    public static event System.Action<Vector2> moveDirectionUpdate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if enemy is walking or running, update move direction
        if (stateMachine.state == stateMachine.walk || stateMachine.state == stateMachine.run)
        {
            moveDirectionUpdate?.Invoke(movement.getDirectionToMove());
        }
    }
}
