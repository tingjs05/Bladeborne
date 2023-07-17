using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // current state
    private PlayerBaseState state;

    // states
    public PlayerIdleState idle {get; private set;}
    public PlayerWalkState walk {get; private set;}
    public PlayerRunState run {get; private set;}
    public PlayerDodgeState dodge {get; private set;}
    public PlayerFirstAttackState attack1 {get; private set;}
    public PlayerSecondAttackState attack2 {get; private set;}
    public PlayerThirdAttackState attack3 {get; private set;}

    // get components
    [HideInInspector] public Rigidbody2D rb;

    // inputs by the player to update every frame
    public Vector2 input {get; private set;}

    // positions of player and mouse
    public Vector2 currentPos {get; private set;}
    public Vector2 mouseWorldPos {get; private set;}

    // weapon and attacks
    [Header("Weapon")]
    [SerializeField] private GameObject weapon;
    public Vector2 mouseDirection {get; private set;}
    public bool isSheathed {get; private set;} = false;

    // sprite
    [Header("Sprite")]
    [SerializeField] private GameObject playerSprite;
    public GameObject weaponSprite {get; private set;}
    public Animator playerAnimator {get; private set;}
    public Animator weaponAnimator {get; private set;}
    private SpriteRenderer playerRenderer;
    private SpriteRenderer weaponRenderer;

    // health
    [field: Header("Health")]
    [field: SerializeField] public float maxHealth {get; private set;} = 1000.0f;
    [HideInInspector] public float health;

    // stamina
    [field: Header("Stamina")]
    [field: SerializeField] public float maxStamina {get; private set;} = 100.0f;
    [field: SerializeField] public float staminaGainPerSecond {get; private set;} = 10.0f;
    [HideInInspector] public float stamina;

    // health and stamina bar
    [field: Header("UI")]
    [field: SerializeField] public GameObject healthBarObject {get; private set;}
    [field: SerializeField] public GameObject staminaBarObject {get; private set;}
    private Bar healthBar;
    private Bar staminaBar;

    // movement
    [field: Header("Movement")]
    [field: SerializeField] public float walkSpeed {get; private set;} = 1.0f;
    [field: SerializeField] public float sprintSpeed {get; private set;} = 2.0f;
    [field: SerializeField] public float staminaDrainPerSecond {get; private set;} = 1.0f;

    // dodge
    [field: Header("Dodge")]
    [field: SerializeField] public float dodgeSpeed {get; private set;} = 15.0f;
    [field: SerializeField] public float dodgeSpeedDropOffScale {get; private set;} = 100.0f;
    [field: SerializeField] public float minDodgeSpeed {get; private set;} = 5.0f;
    [field: SerializeField] public float dodgeStaminaCost {get; private set;} = 20.0f;

    // controls
    public KeyCode sheathKey {get; private set;} = KeyCode.Q;
    public KeyCode attackKey {get; private set;} = KeyCode.Mouse0;
    public KeyCode sprintKey {get; private set;} = KeyCode.LeftShift;
    public KeyCode dodgeKey {get; private set;} = KeyCode.Space;

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
        weaponSprite = weapon.transform.GetChild(0).gameObject;

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
        healthBar.setMax(maxHealth);
        staminaBar.setMax(maxStamina);

        // set input to no input
        input = Vector2.zero;

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
        healthBar.setValue(health);
        staminaBar.setValue(stamina);

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
                weapon.transform.localScale = Vector3.one;
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
            weapon.transform.localScale = Vector3.one;
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
            weapon.transform.localScale = Vector3.one;

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
            weapon.transform.right = Vector2.zero;
            weapon.transform.localScale = Vector3.one;

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
