using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float health;

    void Awake()
    {
        health = maxHealth;    
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0) Die();
    }

    void Die()
    {
        Debug.Log(transform.name + " died.");
    }
}
