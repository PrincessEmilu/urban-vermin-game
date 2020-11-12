using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbstractFightingCharacter
{
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //keep entity upright
        rigidBody.MoveRotation(Quaternion.LookRotation(transform.forward, Vector3.up));
    }

    protected override void ApplyKnockback(float knockBack, int direction)
    {
        //add force to rigidbody
        rigidBody.AddForce(new Vector2(knockBack * direction, 75));

        Debug.Log("ApplyKnockback(): Not implemented yet.");
    }
}
