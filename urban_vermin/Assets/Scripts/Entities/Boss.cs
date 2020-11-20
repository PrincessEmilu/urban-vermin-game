using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractFightingCharacter
{
    private GameObject player;

    public enum State { IDLE, JUMP, WALK, THROW, SLAM, CHARGINGSLAM, CHARGINGTHROW }
    public State state = State.IDLE;
    public int stateTimer = 0;

    public Sprite[] sprites;
    public GameObject slamPrefab;
    public GameObject throwPrefab;

    void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //behavior
        switch (state)
        {
            case State.IDLE:
                setSprite(0);
                if (stateTimer > 50)
                {
                    state = State.WALK;
                    stateTimer = 0;
                }
                break;

            case State.WALK:
                //move towards player
                setSprite(1);
                if (player.transform.position.x < gameObject.transform.position.x)
                {
                    //move left
                    rigidBody.velocity = new Vector2(-1.5f, 0);
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    //move right
                    rigidBody.velocity = new Vector2(1.5f, 0);
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                //check if next to player
                if ((player.transform.position - gameObject.transform.position).magnitude < 2)
                {
                    //stop moving
                    rigidBody.velocity = new Vector2(0, 0);
                    //attack
                    state = State.CHARGINGSLAM;
                    stateTimer = 0;
                }
                //throw if not caught up to player after certain amount of time
                if (stateTimer > 200)
                {
                    state = State.CHARGINGTHROW;
                    stateTimer = 0;
                }
                break;

            case State.SLAM:
                setSprite(2);
                if (stateTimer == 1)
                {
                    Vector2 spawnPoint;
                    if (GetComponent<SpriteRenderer>().flipX) //offset hitbox to left or right
                        spawnPoint = new Vector2(transform.position.x + 2.0f, transform.position.y);
                    else
                        spawnPoint = new Vector2(transform.position.x - 2.0f, transform.position.y);
                    GameObject slamHitbox = Instantiate(slamPrefab, spawnPoint, Quaternion.identity);
                    slamHitbox.GetComponent<DamagingEntity>().sender = gameObject;
                    if (GetComponent<SpriteRenderer>().flipX) //set direction of hitbox
                        slamHitbox.GetComponent<DamagingEntity>().direction = 1;
                    else
                        slamHitbox.GetComponent<DamagingEntity>().direction = -1;
                }
                if (stateTimer >= 30)
                {
                    stateTimer = 0;
                    state = State.IDLE;
                }
                break;

            case State.THROW:
                setSprite(3);
                if (stateTimer == 1)
                {
                    //throw
                    GameObject throwHitbox = Instantiate(throwPrefab, transform.position, Quaternion.identity);
                    throwHitbox.GetComponent<DamagingEntity>().sender = gameObject;
                    if (GetComponent<SpriteRenderer>().flipX) //set direction of hitbox
                        throwHitbox.GetComponent<DamagingEntity>().direction = 1;
                    else
                        throwHitbox.GetComponent<DamagingEntity>().direction = -1;
                }
                if (stateTimer >= 30)
                {
                    stateTimer = 0;
                    state = State.IDLE;
                }
                break;

            case State.CHARGINGSLAM:
                setSprite(4);
                if (stateTimer > 50)
                {
                    stateTimer = 0;
                    state = State.SLAM;
                }
                break;

            case State.CHARGINGTHROW:
                setSprite(5);
                if (stateTimer > 75)
                {
                    stateTimer = 0;
                    state = State.THROW;
                }
                break;
        }
        stateTimer++;

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
