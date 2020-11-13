using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AbstractFightingCharacter
{
    private GameObject player;

    public Sprite[] sprites;

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
            //wind up
            if(attackTimer == 0)
            {
                setSprite(1);
            }

            //create hitbox
            if (attackTimer == 50)
            {
                Vector2 spawnPoint;
                if (GetComponent<SpriteRenderer>().flipX) //offset hitbox to left or right
                    spawnPoint = new Vector2(transform.position.x + 0.5f, transform.position.y);
                else
                    spawnPoint = new Vector2(transform.position.x - 0.5f, transform.position.y);
                GameObject hitbox = Instantiate(hitboxPrefab, spawnPoint, Quaternion.identity);
                if(GetComponent<SpriteRenderer>().flipX) //set direction of hitbox
                    hitbox.GetComponent<DamagingEntity>().direction = 1;
                else
                    hitbox.GetComponent<DamagingEntity>().direction = -1;

                setSprite(2);
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
            setSprite(0);

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

    protected override void ApplyKnockback(float knockBack, int direction)
    {
        //add force to rigidbody
        rigidBody.AddForce(new Vector2(knockBack * direction, 0));
    }

    //0 = idle, 1 = wind up, 2 = swing
    private void setSprite(int index)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[index];
    }
}
