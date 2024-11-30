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
        health = maxHealth;
        shield = maxShield;
    }

    private void Awake()
    {
        Instance = this;
    }

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

        if (health <= 0) GetComponent<PlayerDeath>().Die();
    }
}