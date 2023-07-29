using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchtailStateMachine : MonoBehaviour
{
    // current state
    private ScorchtailBaseState state;

    // states
    public ScorchtailIdleState idle {get; private set;}
    public ScorchtailChaseState chase {get; private set;}
    public ScorchtailPatrolState patrol {get; private set;}
    public ScorchtailStunState stun {get; private set;}
    public ScorchtailRollAttackState rollAtk {get; private set;}
    public ScorchtailTailAttackState tailAtk {get; private set;}
    public ScorchtailScratchAttackState scratchAtk {get; private set;}

    // components
    public Rigidbody2D rb {get; private set;}
    public SpriteRenderer sprite {get; private set;}
    public Animator animator {get; private set;}

    // script references
    public EnemyMovementAI movement {get; private set;}
    public ScorchtailStats stats {get; private set;}

    // inspector properties
    [field: SerializeField] public LayerMask playerLayerMask {get; private set;}
    [field: SerializeField] public LayerMask playerAttackMask {get; private set;}
    [field: SerializeField] public LayerMask obstacleLayerMask {get; private set;}
    [field: SerializeField] public float minRunDistance {get; private set;} = 2.5f;
    [field: SerializeField] public float patrolRange {get; private set;} = 5.0f;
    [field: SerializeField] public float patrolDelay {get; private set;} = 2.0f;
    [field: SerializeField] public float attackRange {get; private set;} = 0.6f;

    // other public properties
    public Vector2 moveDirection {get; private set;} = Vector2.zero;
    public float durationSinceLastPatrol {get; private set;} = 0f;

    void Awake()
    {
        // create instance of each state
        idle = new ScorchtailIdleState();
        chase = new ScorchtailChaseState();
        patrol = new ScorchtailPatrolState();
        stun = new ScorchtailStunState();
        rollAtk = new ScorchtailRollAttackState();
        tailAtk = new ScorchtailTailAttackState();
        scratchAtk = new ScorchtailScratchAttackState();
    }

    // Start is called before the first frame update
    void Start()
    {
        // get components
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();

        //get script reference
        movement = transform.GetChild(1).gameObject.GetComponent<EnemyMovementAI>();
        stats = GetComponent<ScorchtailStats>();

        // set default state
        state = idle;
        state.OnEnter(this);
    }

    // Update is called once per frame
    void Update()
    {
        // update state
        state.OnUpdate(this);

        // update move direction
        moveDirection = movement.getDirectionToMove();

        // increment duration since last patrol
        durationSinceLastPatrol += Time.deltaTime;
    }

    // switch state method
    public void switchState(ScorchtailBaseState newState)
    {
        state.OnExit(this);
        state = newState;
        state.OnEnter(this);
    }

    // reset patrol duration counter
    public void resetPatrolCounter()
    {
        durationSinceLastPatrol = 0f;
    }
}
