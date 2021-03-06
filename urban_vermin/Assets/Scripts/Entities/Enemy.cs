﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AbstractFightingCharacter
{
    private GameObject player;

    private GameObject attackHitbox;

    private bool isAttacking = false;
    private int attackTimer = 0;

    //edit these between different enemies
    public Sprite[] sprites;
    public float speed;
    public float attackDistance;
    public int attackSpeed;
    public float swingDistance;

    public AudioClip attackAudio;

    void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        //behavior
        if (isAttacking)
        {
            //wind up
            if(attackTimer == 1)
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
                attackHitbox = Instantiate(hitboxPrefab, spawnPoint, Quaternion.identity);
                attackHitbox.transform.parent = transform; // Ensure that the hitbox moves with the enemy
                attackHitbox.GetComponent<DamagingEntity>().sender = gameObject;
                if(GetComponent<SpriteRenderer>().flipX) //set direction of hitbox
                    attackHitbox.GetComponent<DamagingEntity>().direction = 1;
                else
                    attackHitbox.GetComponent<DamagingEntity>().direction = -1;

                setSprite(2);
                gameManager.GetComponent<AudioManager>().PlaySFX(attackAudio);
            }

            //end attack
            if (attackTimer >= attackSpeed * 5)
            {
                isAttacking = false;
                attackTimer = 0;
                Destroy(attackHitbox);
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
        if (knockBack > 0)
            rigidBody.AddForce(new Vector2(knockBack * direction, 75));
        //rigidBody.AddForce(new Vector2(knockBack * direction, 0));

    }

    //0 = idle, 1 = wind up, 2 = swing
    private void setSprite(int index)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[index];
    }

    protected override void HandleDeath()
    {
        base.HandleDeath();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (attackHitbox != null)
            Destroy(attackHitbox);
    }
}
