using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AbstractFightingCharacter
{
    private GameObject player;

    private bool isAttacking = false;
    private int attackTimer = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //behavior
        if (isAttacking)
        {
            //attack

            //create hitbox
            if (attackTimer == 0)
            {
                Vector2 spawnPoint;
                if (GetComponent<SpriteRenderer>().flipX) //offset hitbox to left or right
                    spawnPoint = new Vector2(transform.position.x + 0.5f, transform.position.y);
                else
                    spawnPoint = new Vector2(transform.position.x - 0.5f, transform.position.y);
                Instantiate(hitboxPrefab, spawnPoint, Quaternion.identity);
            }

            attackTimer++;

            //end attack
            if (attackTimer >= 100)
            {
                isAttacking = false;
                attackTimer = 0;
            }
        }
        else
        {

            //movement
            if ((player.transform.position - gameObject.transform.position).magnitude < 1)
            {
                //stop moving
                rigidBody.velocity = new Vector2(0, 0);
                //attack
                isAttacking = true;
            }
            else
            {
                //move towards player
                if (player.transform.position.x < gameObject.transform.position.x)
                {
                    //move left
                    rigidBody.velocity = new Vector2(-1, 0);
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    //move right
                    rigidBody.velocity = new Vector2(1, 0);
                    GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }

        //keep entity upright
        rigidBody.MoveRotation(Quaternion.LookRotation(transform.forward, Vector3.up));
    }

    protected override void ApplyKnockback(float knockBack)
    {
        Debug.Log("ApplyKnockback(): Not implemented yet.");
    }
}
