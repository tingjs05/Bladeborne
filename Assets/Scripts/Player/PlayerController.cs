using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // current state
    private PlayerBaseState state;

    // states
    [HideInInspector]
    public PlayerIdleState idle {get; set;}
    [HideInInspector]
    public PlayerWalkState walk {get; set;}
    [HideInInspector]
    public PlayerRunState run {get; set;}
    [HideInInspector]
    public PlayerDodgeState dodge {get; set;}

    // get components
    [HideInInspector]
    public Rigidbody2D rb;

    // inputs by the player to update every frame
    [HideInInspector]
    public Vector2 input;

    // game objects
    [Header("Weapon")]
    public GameObject weapon;

    // sprite
    [Header("Sprite")]
    public GameObject playerSprite;
    [HideInInspector]
    public SpriteRenderer playerRenderer;
    [HideInInspector]
    public Animator playerAnimator;
    [HideInInspector]
    public GameObject weaponSprite;
    [HideInInspector]
    public SpriteRenderer weaponRenderer;
    [HideInInspector]
    public Animator weaponAnimator;

    // stamina
    [Header("Stamina")]
    public float maxStamina = 100.0f;
    public float staminaGainPerSecond = 10.0f;
    [HideInInspector]
    public float stamina = 100.0f;

    // movement
    [Header("Movement")]
    public float walkSpeed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float staminaDrainPerSecond = 1.0f;
    [HideInInspector]
    public KeyCode sprintKey = KeyCode.LeftShift;

    // dodge
    [Header("Dodge")]
    public float dodgeSpeed = 50.0f;
    public float dodgeSpeedDropOffScale = 150.0f;
    public float minDodgeSpeed = 25.0f;
    public float dodgeStaminaCost = 20.0f;
    [HideInInspector]
    public Vector2 currentPos;
    [HideInInspector]
    public Vector2 mouseWorldPos;
    [HideInInspector]
    public KeyCode dodgeKey = KeyCode.Space;

    void Awake()
    {
        // create instance of each state
        idle = new PlayerIdleState();
        walk = new PlayerWalkState();
        run = new PlayerRunState();
        dodge = new PlayerDodgeState();
    }

    // Start is called before the first frame update
    void Start()
    {
        // get components
        rb = GetComponent<Rigidbody2D>();

        // get weapon sprite object from weapon object
        weaponSprite = weapon.transform.GetChild (0).gameObject;

        // get animator components
        playerAnimator = playerSprite.GetComponent<Animator>();
        weaponAnimator = weaponSprite.GetComponent<Animator>();

        // get sprite renderer components
        playerRenderer = playerSprite.GetComponent<SpriteRenderer>();
        weaponRenderer = weaponSprite.GetComponent<SpriteRenderer>();

        // set input to no input
        input = new Vector2(0, 0);

        // set default state
        state = idle;
        state.OnEnter(this);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Stamina: " + stamina);

        // update weapon rotation
        aimWeapon();

        // update input
        updateInput();

        // update state
        state.OnUpdate(this);
    }

    // update variable methods
    public void updateInput()
    {
        // get inputs
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // update input
        input = new Vector2(horizontalInput, verticalInput).normalized;

        // flip player sprite if facing the other way
        if (horizontalInput < 0)
        {
            playerRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            playerRenderer.flipX = false;
        }
    }

    public void updateCurrentPos()
    {
        // get current position of player
        currentPos = new Vector2(transform.position.x, transform.position.y);
    }

    public void updateMousePos()
    {
        // get mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // update world position of mouse
        mouseWorldPos = new Vector2(mousePos.x, mousePos.y);
    }

    // point weapon towards the mouse pointer
    public void aimWeapon()
    {
        // update current and mouse positions
        updateCurrentPos();
        updateMousePos();

        // get direction
        Vector2 direction = mouseWorldPos - currentPos;

        // rotate weapon
        weapon.transform.right = direction;

        // flip weapon when facing the other direction
        if (direction.x < 0)
        {
            weapon.transform.localScale = new Vector3(1, -1, 1);
        }
        else if (direction.x > 0)
        {
            weapon.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // switch state method
    public void switchState(PlayerBaseState newState)
    {
        state.OnExit(this);
        state = newState;
        state.OnEnter(this);
    }
}
