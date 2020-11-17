﻿using System.Collections;
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

    public GameObject bulletPrefab;
    public GameObject flamethrowerInstance;

    // Offsets for spawning the player's attacks - essentially an offset from the prefab's center
    private Vector2 bulletSpawnOffset;
    private Vector2 flamethrowerOffset;

    // The direction this character is facing, 1 for the right, -1 for the left, 0 is unitialized
    private int direction = 0;

    private float walkSpeed;
    private float jumpForce;

    private bool hasJump;
    private bool wasOnGroundLastFrame;

    private ContactFilter2D contactFilter;
    //if (playerCollider.OverlapCollider(contactFilter, collisions) > 0)

    #region Attacking - Properties and Fields
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

    private const int gunWindupMax = 30;
    private const int gunActiveFramesMax = 10;
    private int gunWindup = 0;
    private int gunActiveFrames = 0;

    private const int bashWindupMax = 100;
    private const int staffActiveFramesMax = 10;
    private int bashWindup = 0;
    private int staffActiveFrames = 0;
    #endregion

    public override void Start()
    {
        base.Start();

        flamethrowerInstance.SetActive(false);

        collider = gameObject.GetComponent<CapsuleCollider2D>();
        contactFilter = new ContactFilter2D();

        bulletSpawnOffset = new Vector2(1.0f, 0.0f);
        flamethrowerOffset = new Vector2(2.0f, 0.0f);

        isUsingFlamethrower = false;
        isUsingGun = false;
        isUsingStaff = false;

        hasJump = true;
        wasOnGroundLastFrame = true;
        direction = 1;
        walkSpeed = 7.50f;
        jumpForce = 200.0f;

        health = 100;
        Ammo = maxAmmo;
        Willpower = maxWillpower;

        // Error-checking: Are the bullet and flame attack prefabs assigned?
        if (bulletPrefab == null)
            Debug.LogError("No bullet prefab assigned to player!");

        //if (flamePrefab == null)
           //Debug.LogError("No flame prefab assigned to player!");
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
        }
        else if (Input.GetKey(rightKey))
        {
            direction = 1;
            moveForce.x = walkSpeed;
        }

        // Jump if available
        if (hasJump && Input.GetKeyDown(jumpKey))
        {
            hasJump = false;
            moveForce.y = jumpForce;
        }

        // Apply the movement force
        rigidBody.AddForce(moveForce);

        rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, 10);
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
                if (bashWindup < bashWindupMax)
                {
                    bashWindup++;
                }
                // Windup is done, attack is happening - staff swing happens continuously throughout the attack
                else if (staffActiveFrames < staffActiveFramesMax)
                {
                    SwingStaff();
                    staffActiveFrames++;
                }
            }
        }
    }

    // Fires a single bullet from the gun
    private void ShootGun()
    {
        Vector3 bulletSpawnpoint = transform.position + ( direction * new Vector3(bulletSpawnOffset.x, bulletSpawnOffset.y, 0));

        Bullet newBullet = Instantiate(bulletPrefab, bulletSpawnpoint, Quaternion.identity).GetComponent<Bullet>();
        newBullet.sender = gameObject;
        newBullet.direction = direction;
        newBullet.gameObject.transform.localScale *= direction;
    }

    // Swings the staff over a period of time, has a hitbox "out"
    private void SwingStaff()
    {
        Debug.Log("SWOOSH");
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
                    Debug.Log("Ground Touch!");

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
