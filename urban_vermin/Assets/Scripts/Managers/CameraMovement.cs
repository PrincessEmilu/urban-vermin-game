using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject lookAt;
    public float minX = 0, maxX;
    public bool isFixed;
    public float location;

    void Update()
    {
        if (isFixed)
        {
            transform.position = new Vector3(location, 0, -10);
        }
        else
        {
            float xPos = Mathf.Max(lookAt.transform.position.x, minX);
            xPos = Mathf.Min(xPos, maxX);
            transform.position = new Vector3(xPos, 0, -10);
        }
    }
}
