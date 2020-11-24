using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject cameraObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ignore if player runs into ojbect
        if (collision.gameObject.tag == "Player")
            return;

        //enemy has been hit and destroyed.  Progress to next location
        cameraObject.GetComponent<TutorialCamera>().NextPosition();

        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
