using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingEntity : MonoBehaviour
{
    public float damage;
    public float knockBack;

    void Start()
    {
        damage = 10.0f;
        knockBack = 5.0f;
    }

    //apply damage then delete this
    private void Update()
    {
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
