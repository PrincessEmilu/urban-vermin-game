using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbstractFightingCharacter
{
    private float walkSpeed;
    private float jumpForce;

    private bool hasJump;
    private bool wasOnGroundLastFrame;

    private ContactFilter2D contactFilter;
        //if (playerCollider.OverlapCollider(contactFilter, collisions) > 0)

    public override void Start()
    {
        base.Start();

        collider = gameObject.GetComponent<CapsuleCollider2D>();
        contactFilter = new ContactFilter2D();

        hasJump = true;
        wasOnGroundLastFrame = true;
        walkSpeed = 7.50f;
        jumpForce = 200.0f;

        health = 100;
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
    }

    private void MovePlayer()
    {
        Vector2 moveForce = new Vector2(0, 0);

        // Move left or right
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveForce.x = -walkSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveForce.x = walkSpeed;
        }

        // Jump if available
        if (hasJump && Input.GetKeyDown(KeyCode.UpArrow))
        {
            hasJump = false;
            moveForce.y = jumpForce;
        }

        // Apply the movement force
        rigidBody.AddForce(moveForce);
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
