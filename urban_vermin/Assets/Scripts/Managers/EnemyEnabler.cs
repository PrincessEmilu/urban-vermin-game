using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnabler : MonoBehaviour
{
    public GameObject enemyToEnable;

    void Start()
    {
        enemyToEnable.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            enemyToEnable.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
