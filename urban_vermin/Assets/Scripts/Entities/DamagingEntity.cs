using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingEntity : MonoBehaviour
{
    public float damage;
    public float knockBack;
    public int direction = 0;
    public bool destroyOnApplyDamage = false;
    public bool disableOnApplyDamage = false;
    public GameObject sender;
    protected Rigidbody2D rigidBody;

    protected bool appliedDamage;


    protected virtual void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        appliedDamage = false;
    }

    //apply damage then delete this
    protected virtual void Update()
    {
        //wait until direction has been assigned before checking for hits
        if (direction == 0)
            return;

        CheckForDamage(GetComponent<Collider2D>());

        if (appliedDamage)
        {
            if (destroyOnApplyDamage)
                Destroy(gameObject);
            else if (disableOnApplyDamage)
                gameObject.SetActive(false);
        }
    }

    protected void CheckForDamage(Collider2D collider)
    {
        //check for hits
        GameObject[] allObjs = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjs)
        {
            if (obj.GetComponent<AbstractFightingCharacter>() != null) //only check enemies/player
            {
                if (collider.IsTouching(obj.GetComponent<Collider2D>())) //if touching
                {
                    if (obj != sender) //dont hit yourself
                    {
                        obj.GetComponent<AbstractFightingCharacter>().TakeDamage(gameObject);
                        appliedDamage = true;
                    }
                }
            }

            if (gameObject.tag == "PlayerMelee") //this is player melee
            {
                if (obj.GetComponent<BossProjectile>() != null) //destroy boss projectile
                {
                    if (collider.IsTouching(obj.GetComponent<Collider2D>())) //if touching
                    {
                        Debug.Log("Deflected");
                        Destroy(obj);
                    }
                }
            }
        }
    }

}
