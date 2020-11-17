using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractFightingCharacter
{
    private GameObject player;

    private bool isAttacking = false;
    private int attackTimer = 0;

    public Sprite[] sprites;
    public float speed = 1;
    public float attackDistance = 1;
    public int attackSpeed = 50;
    public float swingDistance = 1;

    void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //behavior
        if (isAttacking)
        {
            //wind up
            if (attackTimer == 1)
            {
                setSprite(1);
            }

            //create hitbox
            if (attackTimer == attackSpeed)
            {
                Vector2 spawnPoint;
                if (GetComponent<SpriteRenderer>().flipX) //offset hitbox to left or right
                    spawnPoint = new Vector2(transform.position.x + swingDistance, transform.position.y);
                else
                    spawnPoint = new Vector2(transform.position.x - swingDistance, transform.position.y);
                GameObject hitbox = Instantiate(hitboxPrefab, spawnPoint, Quaternion.identity);
                hitbox.GetComponent<DamagingEntity>().sender = gameObject;
                if (GetComponent<SpriteRenderer>().flipX) //set direction of hitbox
                    hitbox.GetComponent<DamagingEntity>().direction = 1;
                else
                    hitbox.GetComponent<DamagingEntity>().direction = -1;

                setSprite(2);
            }

            //end attack
            if (attackTimer >= attackSpeed * 2)
            {
                isAttacking = false;
                attackTimer = 0;
            }

            attackTimer++;
        }
        else
        {
            setSprite(0);

            //movement
            if ((player.transform.position - gameObject.transform.position).magnitude < attackDistance)
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
                    rigidBody.velocity = new Vector2(-speed, 0);
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    //move right
                    rigidBody.velocity = new Vector2(speed, 0);
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
        //rigidBody.AddForce(new Vector2(knockBack * direction, 75));
        rigidBody.AddForce(new Vector2(knockBack * direction, 0));

    }

    //0 = idle, 1 = wind up, 2 = swing
    private void setSprite(int index)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[index];
    }

}
