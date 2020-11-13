using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject lookAt;
    public int maxX;

    void Update()
    {
        float xPos = Mathf.Max(lookAt.transform.position.x, 0.0f);
        xPos = Mathf.Min(xPos, maxX);
        transform.position = new Vector3(xPos, 0, -10);
    }
}
