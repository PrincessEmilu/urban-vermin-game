using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFightingCharacter : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject hitboxPrefab;

    public const float MaxHealth = 100.0f;

    public float health;
    protected Rigidbody2D rigidBody;
    protected Collider2D colliderObj;

    public virtual void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<GameManager>();

        if (gameManager == null)
            Debug.LogError("GameManager not found!");
    }

    public virtual void TakeDamage(GameObject damagingObject)
    {
        DamagingEntity damageSource = damagingObject.GetComponent<DamagingEntity>();

        ApplyDamage(damageSource.damage);
        ApplyKnockback(damageSource.knockBack, damageSource.direction);
    }
    protected virtual void ApplyDamage(float damage)
    {
        health -= damage;
        //Debug.Log(gameObject.name + " has taken " + damage + " damage!");

        if (health <= 0)
            HandleDeath();
    }

    protected virtual void HandleDeath()
    {
        //Debug.Log(gameObject.name + " has died!");
    }
    protected abstract void ApplyKnockback(float knockBack, int direction);

}
