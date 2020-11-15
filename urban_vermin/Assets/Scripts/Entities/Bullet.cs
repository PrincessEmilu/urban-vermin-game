using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : DamagingEntity
{
    public float movespeed = 1000;

    protected override void Update()
    {
        base.Update();

        rigidBody.AddForce(new Vector2(movespeed * direction, 0));
    }
}
