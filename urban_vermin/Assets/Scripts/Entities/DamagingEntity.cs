using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingEntity : MonoBehaviour
{
    public float damage;
    public float knockBack;
    public int direction = 0;

    //apply damage then delete this
    private void Update()
    {
        //wait until direction has been assigned before checking for hits
        if (direction == 0)
            return;

        //check for hits
        GameObject[] allObjs = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjs)
        {
            if (obj.GetComponent<AbstractFightingCharacter>() != null) //only check enemies/player
            {
                if (GetComponent<Collider2D>().IsTouching(obj.GetComponent<Collider2D>())) //if touching
                {
                    obj.GetComponent<AbstractFightingCharacter>().TakeDamage(gameObject);
                }
            }
        }

        //delete this
        Destroy(this.gameObject);
    }

}
