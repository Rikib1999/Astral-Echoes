using Assets.Scripts;
using Assets.Scripts.Player;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    private int health;
    private int shield;
    public static ShipHealth Instance;
    [SerializeField] int maxHealth;
    [SerializeField] int maxShield;

    private void Start()
    {
        //init players ship health
        health = maxHealth;
        shield = maxShield;
    }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Damage to the player
    /// </summary>
    /// <param name="damage"></param>
    public void damage(int damage)
    {
        if (shield > 0) {
            health -= damage/(shield/2);
            Debug.Log("Player Health after shield: " + health);
            shield -= damage;
        }
        else
        {
            health -= damage;
            Debug.Log("Player Health: " + health);
        }

        if (health <= 0)
        {
            ScoreManager.IncrementKillCount();
            GetComponent<PlayerDeath>().Die();
        }
    }
}