using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchtailStateMachine : MonoBehaviour
{
    // current state
    public ScorchtailBaseState state {get; private set;}

    // states
    public ScorchtailIdleState idle {get; private set;}
    public ScorchtailWalkState walk {get; private set;}
    public ScorchtailRunState run {get; private set;}
    public ScorchtailStunState stun {get; private set;}
    public ScorchtailRollAttackState rollAtk {get; private set;}
    public ScorchtailTailAttackState tailAtk {get; private set;}
    public ScorchtailScratchAttackState scratchAtk {get; private set;}

    // inspector fields
    [field: SerializeField] public SpriteRenderer sprite {get; private set;}
    [field: SerializeField] public Animator animator {get; private set;}
    [field: SerializeField] public ScorchtailStats stats {get; private set;}

    // components
    public Rigidbody2D rb {get; private set;}

    void Awake()
    {
        // create instance of each state
        idle = new ScorchtailIdleState();
        walk = new ScorchtailWalkState();
        run = new ScorchtailRunState();
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

        // set default state
        state = idle;
        state.OnEnter(this);
    }

    // Update is called once per frame
    void Update()
    {
        // update state
        state.OnUpdate(this);
    }

    // switch state method
    public void switchState(ScorchtailBaseState newState)
    {
        state.OnExit(this);
        state = newState;
        state.OnEnter(this);
    }
}
