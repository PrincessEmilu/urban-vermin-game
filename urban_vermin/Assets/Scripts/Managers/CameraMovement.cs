using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject lookAt;
    public int maxX;
    public bool isTutorial; //If true, interpolates on location, else looksat between 0 and maxX
    public int location;

    void Update()
    {
        if (isTutorial)
        {
            transform.position = new Vector3(location, 0, -10);
        }
        else
        {
            float xPos = Mathf.Max(lookAt.transform.position.x, 0.0f);
            xPos = Mathf.Min(xPos, maxX);
            transform.position = new Vector3(xPos, 0, -10);
        }
    }
}
