using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : AbstractFightingCharacter
{
    public GameObject player;
    public bool entranceAnimation = true;
    public float dying = -1.0f;
    public GameObject healthBar;

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
        healthBar.SetActive(false);
    }

    void Update()
    {
        if(dying > -1.0f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, dying);
            if (dying <= 0)
                SceneManager.LoadScene("VictoryScene");
            else
            {
                dying -= 0.01f;
                return;
            }
        }

        if (entranceAnimation)
        {
            if (transform.position.y > 0)
                return;
            else
            {
                healthBar.SetActive(true);
                entranceAnimation = false;
            }
        }

        healthBar.transform.GetChild(0).GetComponent<Image>().fillAmount = health / 1500.0f;

        stateTimer++;
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
                        spawnPoint = new Vector2(transform.position.x + 1.0f, transform.position.y);
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
                    Vector3 spawn = new Vector3(transform.position.x, transform.position.y + 1.0f, 0.5f);
                    GameObject throwHitbox = Instantiate(throwPrefab, spawn, Quaternion.identity);
                    if (GetComponent<SpriteRenderer>().flipX) //set direction of hitbox
                        throwHitbox.GetComponent<BossProjectile>().direction = 1;
                    else
                        throwHitbox.GetComponent<BossProjectile>().direction = -1;
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

        //keep entity upright
        rigidBody.MoveRotation(Quaternion.LookRotation(transform.forward, Vector3.up));

        //check for death
        if(health <= 0)
        {
            dying = 1.0f;
        }
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
