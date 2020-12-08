using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : DamagingEntity
{
    public float movespeed;
    private int lifeTime = 0;

    protected override void Start()
    {
        base.Start();

        rigidBody.AddForce(new Vector2(movespeed * direction, 0));
    }
    protected override void Update()
    {
        base.Update();

        if (appliedDamage)
            Destroy(gameObject);

        if (lifeTime > 30)
            Destroy(gameObject);
        lifeTime++;
    }
}
