using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public int direction;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //apply initial force
        if(direction != 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction * 10, 0);
            direction = 0;
        }

        //player
        if (GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>()))
        {
            player.GetComponent<Player>().TakeDamage(25.0f);
            Destroy(gameObject);
        }

        //hit ground
        if (transform.position.y < -2.5)
            Destroy(gameObject);
    }
}
