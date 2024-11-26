using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] int maxHealth;
    [SerializeField] GameObject Player;

    private void Start()
    {
        if (Player == null) { Player = GameObject.FindWithTag("Player"); }
        health = maxHealth;
    }
    public void damage(int damage)
    {
        Debug.Log("HIT");
        if (health >= 0)
        {
            health -= damage;
            Debug.Log(health);
        }
        if (health < 0) { Destroy(gameObject); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.GetComponent<PlayerLogic>().damage(10);

        }
    }
}
