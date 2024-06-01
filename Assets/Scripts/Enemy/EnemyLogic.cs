using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
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
            Debug.Log(health);
        }
        if (health < 0) { Destroy(gameObject); }
    }
}
