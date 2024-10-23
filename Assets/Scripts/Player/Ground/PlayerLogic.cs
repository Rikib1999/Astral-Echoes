using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public static PlayerLogic Instance;
    private int health;
    [SerializeField] int maxHealth;

    private void Start()
    {
        health = maxHealth;
    }

    public void Update()
    {
        //Debug.Log(health);
    }

    public void Awake()
    {
        Instance = this;
    }

    public void IncreaseHealth(int value)
    {
        health += value;
        Debug.Log(health);
    }

    public void damage(int damage)
    {

        health -= damage;
        Debug.Log("Player Health: " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
