using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnabler : MonoBehaviour
{
    public GameObject enemyToEnable;
    public bool lockScreen; //if true, stop camera movement when this is triggered (for boss level)

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
            if(lockScreen)
            {
                Camera cam = FindObjectOfType<Camera>();
                cam.GetComponent<CameraMovement>().location = cam.transform.position.x;
                cam.GetComponent<CameraMovement>().isFixed = true;
            }
        }
    }
}
