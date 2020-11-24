using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    public GameObject cameraObject;
    public CameraMovement cameraScript;
    public List<int> cameraPositions;
    public int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        cameraScript = cameraObject.GetComponent<CameraMovement>();
    }

    public void NextPosition()
    {
        index++;
        cameraScript.location = cameraPositions[index];
    }

}
