using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbstractFightingCharacter
{
    #region Define Keyboard Controls
    // Movement
    private const KeyCode jumpKey = KeyCode.UpArrow;
    private const KeyCode leftKey = KeyCode.LeftArrow;
    private const KeyCode rightKey = KeyCode.RightArrow;

    // Attacks
    private const KeyCode bashKey = KeyCode.Z;
    private const KeyCode gunKey = KeyCode.X;
    private const KeyCode fireKey = KeyCode.C;
    #endregion

    #region Movement Fields
    // The direction this character is facing, 1 for the right, -1 for the left, 0 is unitialized
    private int direction = 0;

    private const float velocityMax = 5.0f;
    private float walkSpeed;
    private float jumpForce;

    private bool isWalking;
    private bool hasJump;
    private bool wasOnGroundLastFrame;

    private ContactFilter2D contactFilter;
    //if (playerCollider.OverlapCollider(contactFilter, collisions) > 0)
    #endregion

    #region Attacking - Properties and Fields
    public GameObject bulletPrefab;
    public GameObject flamethrowerInstance;

    // Offsets for spawning the player's attacks - essentially an offset from the prefab's center
    private Vector2 bulletSpawnOffset;
    private Vector2 flamethrowerOffset;
    public int Ammo { get; private set; }
    public float Willpower { get; private set; }

    public bool IsAttacking
    { 
        get
        {
            return isUsingStaff || isUsingGun || isUsingFlamethrower;
        }
    }

    private bool isUsingFlamethrower;
    private bool isUsingGun;
    private bool isUsingStaff;

    // Willpower cost per frame
    private float flamethrowerCost = 0.1f;

    private const int maxAmmo = 24;
    private const float maxWillpower = 100.0f;
    private const float willpowerRechargeRate = 0.25f;

    private const int gunWindupMax = 10;
    private const int gunActiveFramesMax = 10;
    private int gunWindup = 0;
    private int gunActiveFrames = 0;

    private const int staffWindupMax = 10;
    private const int staffActiveFramesMax = 30;
    private int staffWindupFrames = 0;
    private int staffActiveFrames = 0;
    #endregion

    #region Sprites
    private SpriteRenderer spriteRenderer;

    // TODO: Once all frames are in, this will need to be handled differently.
    [SerializeField]
    private Sprite gunSprite;
    [SerializeField]
    private Sprite meleeSprite;
    [SerializeField]
    private Sprite flameSprite;

    [SerializeField]
    private Sprite idleSprite;
    [SerializeField]
    private Sprite walkSprite1;
    [SerializeField]
    private Sprite walkSprite2;
    [SerializeField]
    private Sprite damageSprite;
    #endregion

    public override void Start()
    {
        base.Start();

        flamethrowerInstance.SetActive(false);

        collider = gameObject.GetComponent<BoxCollider2D>();
        contactFilter = new ContactFilter2D();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        bulletSpawnOffset = new Vector2(1.5f, 0.9f);
        flamethrowerOffset = new Vector2(2.75f, 0.5f);

        isUsingFlamethrower = false;
        isUsingGun = false;
        isUsingStaff = false;

        isWalking = false;
        hasJump = true;
        wasOnGroundLastFrame = true;
        direction = 1;
        walkSpeed = 10.0f;
        jumpForce = 1000.0f;

        health = 100;
        Ammo = maxAmmo;
        Willpower = maxWillpower;

        // Error-checking: Are the bullet and flame attack prefabs assigned?
        if (bulletPrefab == null)
            Debug.LogError("No bullet prefab assigned to player!");

        if (collider == null)
           Debug.LogError("No collider assignhed to player!");

    }

    private void Update()
    {
        //keep entity upright
        rigidBody.MoveRotation(Quaternion.LookRotation(transform.forward, Vector3.up));

        // Refresh the player's jump
        if (!hasJump)
            CheckGrounded();
        //Invoke("CheckGrounded", 1.0f);

        // Move the player based on input
        MovePlayer();

        // Process keyboard input and manage attacks
        HandleAttacks();

        // Handle animations
        HandleSpriteChange();

        // Recharge willpower if not full or being used this frame
        if (Willpower < maxWillpower && !isUsingFlamethrower)
            Willpower += willpowerRechargeRate;
    }

    private void MovePlayer()
    {
        Vector2 moveForce = new Vector2(0, 0);

        // Move left or right
        if (Input.GetKey(leftKey))
        {
            direction = -1;
            moveForce.x = -walkSpeed;
            isWalking = true;
        }
        else if (Input.GetKey(rightKey))
        {
            direction = 1;
            moveForce.x = walkSpeed;
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        // Jump, if available
        if (hasJump && Input.GetKeyDown(jumpKey))
        {
            hasJump = false;
            moveForce.y = jumpForce;
        }

        // Apply the movement force
        rigidBody.AddForce(moveForce);

        // Clamp velocity so Dresden doesn't go flying
        rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, velocityMax);
    }

    // Checks the player's key presses and handles doing the appropriate attacks
    private void HandleAttacks()
    {
        // Begin attacking if an attack isn't in progress
        if (!IsAttacking)
        {
            // First press of the firespell key
            if (Input.GetKeyDown(fireKey))
            {
                isUsingFlamethrower = true;

                flamethrowerInstance.SetActive(true);
                Flamethrower flameThrowerScript = flamethrowerInstance.GetComponent<Flamethrower>();
                flameThrowerScript.direction = direction;
            }
            // First press of the gun
            else if (!isUsingGun && Input.GetKeyDown(gunKey))
            {
                isUsingGun = true;
            }
            // First swing of the staff
            else if (Input.GetKeyDown(bashKey))
            {
                isUsingStaff = true;
            }
        }

        // Update attacks that are in progress
        if (IsAttacking)
        {
            // Flamethrower is being used
            if (isUsingFlamethrower)
            {
                Debug.Log("Is using flamethrower");
                // Stop using the flamethrower if out of willpower or key isn't being held
                if (!Input.GetKey(fireKey) || Willpower == 0)
                {
                    flamethrowerInstance.SetActive(false);
                    isUsingFlamethrower = false;
                }
                else
                {
                    Flamethrower flameThrowerScript = flamethrowerInstance.GetComponent<Flamethrower>();
                    flameThrowerScript.offsetVector = new Vector3(flamethrowerOffset.x, flamethrowerOffset.y, -1);
                    Willpower -= flamethrowerCost;
                }
            }
            // If the gun is being used
            if (isUsingGun)
            {
                // Windup is still happening
                if (gunWindup < gunWindupMax)
                {
                    gunWindup++;

                    // TODO: Animation stuff, presumably
                }
                // Windup done, shoot the gun
                else if (gunActiveFrames == 0)
                {
                    ShootGun();
                    gunActiveFrames++;
                }
                // Attack animation is playing out
                else if (gunActiveFrames < gunActiveFramesMax)
                {
                    gunActiveFrames++;

                    // TODO: Animation stuff, presumably
                }
                // Attack animation is totally done, reset for next attack
                else
                {
                    gunActiveFrames = 0;
                    gunWindup = 0;
                    isUsingGun = false;
                }
            }
            else if (isUsingStaff)
            {
                // Windup is still happening
                if (staffWindupFrames < staffWindupMax)
                {
                    staffWindupFrames++;
                }
                // Windup is done, attack is happening - staff swing happens continuously throughout the attack
                else if (staffActiveFrames < staffActiveFramesMax)
                {
                    SwingStaff();
                    staffActiveFrames++;
                }
                else
                {
                    staffActiveFrames = 0;
                    staffWindupFrames = 0;
                    isUsingStaff = false;
                }
            }
        }
    }

    // Fires a single bullet from the gun
    private void ShootGun()
    {
        Vector3 bulletSpawnpoint = transform.position + new Vector3(direction * bulletSpawnOffset.x, bulletSpawnOffset.y, 0);

        Bullet newBullet = Instantiate(bulletPrefab, bulletSpawnpoint, Quaternion.identity).GetComponent<Bullet>();
        newBullet.sender = gameObject;
        newBullet.direction = direction;
        newBullet.gameObject.transform.localScale *= direction;
    }

    // Swings the staff over a period of time, has a hitbox "out"
    private void SwingStaff()
    {
        //Debug.Log("SWOOSH");
    }

    // Changes the sprite based on what the player is doing currently
    private void HandleSpriteChange()
    {
        if (isUsingGun)
            spriteRenderer.sprite = gunSprite;
        else if (isUsingFlamethrower)
            spriteRenderer.sprite = flameSprite;
        else if (isUsingStaff)
            spriteRenderer.sprite = meleeSprite;
        else if (isWalking)
            spriteRenderer.sprite = walkSprite1;
        else
            spriteRenderer.sprite = idleSprite;

        // TODO: Handle damaged animation

        // Flip sprite based on direction
        if (direction == -1)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    private void CheckGrounded()
    {
        // Give the player their jump back if they are touching the ground
        List<Collider2D> collisions = new List<Collider2D>();
        if (collider.GetContacts(collisions) > 0)
        {
            for (int i = 0; i < collisions.Count; i++)
            {
                if (collisions[i].gameObject.tag == "Ground")
                {
                    hasJump = true;
                }
            }
        }
    }

    protected override void ApplyKnockback(float knockBack, int direction)
    {
        //add force to rigidbody
        rigidBody.AddForce(new Vector2(knockBack * direction, 75));
    }
}
