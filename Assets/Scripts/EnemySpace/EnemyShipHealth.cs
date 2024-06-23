using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipHealth : MonoBehaviour
{
    private int health;
    private int shield;
    [SerializeField] int maxHealth;
    [SerializeField] int maxShield;

    private void Start()
    {
        health = maxHealth;
        shield = maxShield;
    }
    public void damage(int damage)
    {

        /*if (health > maxHealth)
        {
            health -= damage;
            
        }
        if(health <= 0) { Destroy(gameObject); }*/
        if (shield > 0)
        {
            Debug.Log("Enemy Health: " + health);

            Debug.Log("Enemy Shield: " + shield);
            health -= damage / (shield / 2);
            Debug.Log("Enemy Health after shield: " + health);
            shield -= damage;
        }
        else
        {
            health -= damage;
            Debug.Log("Enemy Health: " + health);

        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}