using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Crate : AbstractFightingCharacter // I am aware that technically, a crate is not a fighter. But it could be!
{
    [SerializeField]
    private GameObject[] spawnables;

    [SerializeField]
    private float spawnChance = 0.25f;

    [SerializeField]
    private AudioClip breakClip;

    public override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(GameObject damagingObject)
    {
        if (damagingObject.tag != "PlayerAttack")
        {
            Debug.Log(damagingObject.tag);
            return;
        }

        // Chance to spawn a collectible
        float randomNumber = Random.Range(0.0f, 1.0f);
        if (randomNumber <= spawnChance)
        {
            //Debug.Log(string.Format("How is {0} less than {1}", randomNumber, spawnChance));
            Instantiate(spawnables[(int)System.Math.Round(Random.Range(0.0f, 1.0f))], transform.position, Quaternion.identity);
        }

        gameManager.GetComponent<GameManager>().PlaySound(breakClip);
        Destroy(gameObject);
    }

    protected override void ApplyDamage(float damage)
    {
        return;
    }

    protected override void ApplyKnockback(float knockBack, int direction)
    {
        return;
    }
}
