using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AbstractFightingCharacter
{
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //movement
        if ((player.transform.position - gameObject.transform.position).magnitude < 1)
        {
            //stop moving
            rigidBody.velocity = new Vector2(0, 0);
            //attack
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
        //keep entity upright
        rigidBody.MoveRotation(Quaternion.LookRotation(transform.forward, Vector3.up));
    }

    protected override void ApplyKnockback(float knockBack)
    {
        Debug.Log("ApplyKnockback(): Not implemented yet.");
    }
}
