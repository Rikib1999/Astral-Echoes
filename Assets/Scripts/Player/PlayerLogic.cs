using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private int health;
    [SerializeField] int maxHealth;

    private void Start()
    {
        health = maxHealth;
    }
    public void damage(int damage)
    {
        if (health > maxHealth)
        {
            health -= damage;
        }
        if(health <= 0) { Destroy(gameObject); }
    }
}
