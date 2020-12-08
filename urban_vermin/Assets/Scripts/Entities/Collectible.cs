using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    public abstract void OnCollect(Player player);

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            OnCollect(collision.gameObject.GetComponent<Player>());
    }
}
