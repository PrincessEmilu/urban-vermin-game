using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : DamagingEntity
{
    public Vector3 offsetVector;

    // Update is called once per frame
    protected override void Update()
    {
        transform.position = sender.transform.position + (direction * offsetVector);
        base.Update();
    }
}
