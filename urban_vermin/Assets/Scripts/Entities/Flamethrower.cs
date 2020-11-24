using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : DamagingEntity
{
    public Vector3 offsetVector;
    private int previousDirection = 1;

    // Update is called once per frame
    protected override void Update()
    {
        Vector3 newPosition = sender.transform.position;
        newPosition.x += (direction * offsetVector.x);
        newPosition.y += offsetVector.y;
        transform.position = newPosition;
        if (direction != previousDirection)
        {
            previousDirection = direction;
            gameObject.transform.localScale *= direction;
        }
            
        base.Update();
    }
}
