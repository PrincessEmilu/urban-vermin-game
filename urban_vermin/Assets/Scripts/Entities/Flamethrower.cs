using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : DamagingEntity
{
    public Vector3 offsetVector;
    private int previousDirection = 1;
    private int activeTime = 0;

    [SerializeField]
    private BoxCollider2D nearCollider;
    [SerializeField]
    private BoxCollider2D midCollider;
    [SerializeField]
    private BoxCollider2D farCollider;

    // Update is called once per frame
    protected override void Update()
    {
        activeTime++;

        UpdatePositionAndScale();

        // Waits for flamethrower to come out visually to use the different hitboxes
        if (activeTime > 10)
            CheckForDamage(nearCollider);
        if (activeTime > 30)
            CheckForDamage(midCollider);
        if (activeTime > 60)
            CheckForDamage(farCollider);
    }

    public void OnDisable()
    {
        activeTime = 0;
    }

    protected void UpdatePositionAndScale()
    {
        Vector3 newScale = transform.localScale;
        Vector3 newPosition = sender.transform.position;
        newPosition.x += (direction * offsetVector.x);
        newPosition.y += offsetVector.y;

        if (direction != previousDirection)
        {
            previousDirection = direction;
            //newScale.x *= direction;
            newScale *= -1;
        }

        Debug.Log(newScale);
        transform.position = newPosition;
        transform.localScale = newScale;
    }
}
