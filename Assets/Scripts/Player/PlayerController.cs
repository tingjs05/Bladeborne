using System;
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
    [HideInInspector]
    public PlayerFirstAttackState attack1 {get; set;}
    [HideInInspector]
    public PlayerSecondAttackState attack2 {get; set;}
    [HideInInspector]
    public PlayerThirdAttackState attack3 {get; set;}

    // get components
    [HideInInspector]
    public Rigidbody2D rb;

    // inputs by the player to update every frame
    [HideInInspector]
    public Vector2 input;

    // weapon and attacks
    [Header("Weapon")]
    public GameObject weapon;
    [HideInInspector]
    public Vector2 mouseDirection;
    [HideInInspector]
    public bool isSheathed = false;
    [HideInInspector]
    public KeyCode sheathKey = KeyCode.Q;
    [HideInInspector]
    public KeyCode attackKey = KeyCode.Mouse0;

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

    // health
    [Header("Health")]
    public float maxHealth = 1000.0f;
    [HideInInspector]
    public float health;

    // stamina
    [Header("Stamina")]
    public float maxStamina = 100.0f;
    public float staminaGainPerSecond = 10.0f;
    [HideInInspector]
    public float stamina;

    // health and stamina bar
    [Header("UI")]
    public GameObject healthBarObject;
    public GameObject staminaBarObject;
    private Bar healthBar;
    private Bar staminaBar;

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
        attack1 = new PlayerFirstAttackState();
        attack2 = new PlayerSecondAttackState();
        attack3 = new PlayerThirdAttackState();
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

        // get bar component
        healthBar = healthBarObject.GetComponent<Bar>();
        staminaBar = staminaBarObject.GetComponent<Bar>();

        // set health and stamina to max
        health = maxHealth;
        stamina = maxStamina;

        // set health and stamina bar
        healthBar.setMax((int) Math.Round(maxHealth));
        staminaBar.setMax((int) Math.Round(maxStamina));

        // set input to no input
        input = new Vector2(0, 0);

        // set default state
        state = idle;
        state.OnEnter(this);

        // start with sheathed weapon
        toggleSheath();
    }

    // Update is called once per frame
    void Update()
    {
        // update health and stamina bar
        healthBar.setValue((int) Math.Round(health));
        staminaBar.setValue((int) Math.Round(stamina));

        // update state
        state.OnUpdate(this);
    }

    // update variable methods
    public void updateInput(bool flip = true)
    {
        // get inputs
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // update input
        input = new Vector2(horizontalInput, verticalInput).normalized;

        // exit function if don't need to flip player and weapon according to input
        if (!flip)
        {
            return;
        }

        // flip player sprite if facing the other way
        if (horizontalInput < 0)
        {
            playerRenderer.flipX = true;

            // flip weapon as well if sheathed
            if (isSheathed)
            {
                weapon.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else if (horizontalInput > 0)
        {
            playerRenderer.flipX = false;

            // flip weapon as well if sheathed
            if (isSheathed)
            {
                weapon.transform.localScale = new Vector3(1, 1, 1);
            }
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
        mouseDirection = mouseWorldPos - currentPos;

        // rotate weapon
        weapon.transform.right = mouseDirection;

        // flip weapon and player when facing the other direction
        if (mouseDirection.x < 0)
        {
            weapon.transform.localScale = new Vector3(1, -1, 1);
            playerRenderer.flipX = true;
        }
        else if (mouseDirection.x > 0)
        {
            weapon.transform.localScale = new Vector3(1, 1, 1);
            playerRenderer.flipX = false;
        }

        //make weapon go behind player if above player
        if (mouseDirection.y > 0.5f)
        {
            weaponRenderer.sortingOrder = -1;
        }
        else
        {
            weaponRenderer.sortingOrder = 1;
        }
    }

    // sheath weapon
    public void toggleSheath()
    {
        if (isSheathed)
        {
            // unsheath weapon if sheathed
            isSheathed = false;

            // change to idle animation
            weaponAnimator.Play("Weapon_Idle");

            // reset adjusted position of weapon
            weaponSprite.transform.Translate(new Vector3(0.1f, 0f, 0f));

            // reset weapon flipX if weapon was flipped when sheathed
            weapon.transform.localScale = new Vector3(1, 1, 1);

            // make weapon appear in front of player
            weaponRenderer.sortingOrder = 1;
        }
        else
        {
            // sheath weapon if unsheathed
            isSheathed = true;

            // change to sheath animation
            weaponAnimator.Play("Weapon_Sheath");

            // reset weapon rotation before sheathing weapon
            weapon.transform.right = new Vector2(0, 0);
            weapon.transform.localScale = new Vector3(1, 1, 1);

            // adjust position of weapon
            weaponSprite.transform.Translate(new Vector3(-0.1f, 0f, 0f));

            // make weapon appear behind the player
            weaponRenderer.sortingOrder = -1;
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
