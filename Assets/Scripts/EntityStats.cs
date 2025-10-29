using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHealth;
    private int currentHealth;

    public Stat strength;
    public Stat damage;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(EntityStats entity)
    {
        int dmg = damage.GetValue() + strength.GetValue();

        entity.TakeDamage(dmg);
    }

    public virtual void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {

    }
}
