using System.Collections;
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


    private float walkSpeed;
    private float jumpForce;

    private bool hasJump;
    private bool wasOnGroundLastFrame;

    private ContactFilter2D contactFilter;
        //if (playerCollider.OverlapCollider(contactFilter, collisions) > 0)

    public int Ammo { get; private set; }
    public float Willpower { get; private set; }

    private bool isUsingSpell;

    private const int maxAmmo = 24;
    private const float maxWillpower = 100.0f;
    private const float willpowerRechargeRate = 0.25f;

    public override void Start()
    {
        base.Start();

        collider = gameObject.GetComponent<CapsuleCollider2D>();
        contactFilter = new ContactFilter2D();

        hasJump = true;
        isUsingSpell = false;
        wasOnGroundLastFrame = true;
        walkSpeed = 7.50f;
        jumpForce = 200.0f;

        health = 100;
        Ammo = maxAmmo;
        Willpower = maxWillpower;
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
        if (Willpower < maxWillpower && !isUsingSpell)
            Willpower += willpowerRechargeRate;
    }

    private void MovePlayer()
    {
        Vector2 moveForce = new Vector2(0, 0);

        // Move left or right
        if (Input.GetKey(leftKey))
        {
            moveForce.x = -walkSpeed;
        }
        else if (Input.GetKey(rightKey))
        {
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
    }

    // Checks the player's key presses and handles doing the appropriate attacks
    private void HandleAttacks()
    {
        if (Input.GetKeyDown(fireKey))
        {

        }
        else if (Input.GetKeyDown(gunKey))
        {

        }
        else if (Input.GetKeyDown(bashKey))
        {

        }
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
