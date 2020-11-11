using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFightingCharacter : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject hitboxPrefab;

    public const float MaxHealth = 100.0f;

    protected float health;
    protected Rigidbody2D rigidBody;
    protected BoxCollider2D boxCollider;

    public virtual void TakeDamage(GameObject damagingObject)
    {
        DamagingEntity damageSource = damagingObject.GetComponent<DamagingEntity>();

        ApplyDamage(damageSource.damage);
        ApplyKnockback(damageSource.knockBack);
    }
    protected virtual void ApplyDamage(float damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " has taken " + damage + " damage!");
        if (health <= 0)
            Debug.Log(gameObject.name + " has died!");
    }
    protected abstract void ApplyKnockback(float knockBack);

}
