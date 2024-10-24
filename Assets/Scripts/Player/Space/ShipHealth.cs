using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    private int health;
    private int shield;
    private float fuel;
    public float fuelConsumptionRate = 5f;
    public static ShipHealth Instance;
    [SerializeField] int maxHealth;
    [SerializeField] int maxShield;
    [SerializeField] float maxFuel;

    private void Start()
    {
        health = maxHealth;
        shield = maxShield;
        fuel = maxFuel;
    }
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {

        if(Input.GetKey(KeyCode.W))
        {
            fuel -= fuelConsumptionRate * Time.deltaTime;
            //getFuel(fuel);
            
        }
        if(fuel <= 0)
        {
            fuel = 0;
        }

        //Debug.Log("Fuel: " + fuel);

    }

    public float getFuel()
    {
        return fuel;
    }

    public void IncreaseFuel(float fill)
    {
        fuel += fill;

    }

    public void damage(int damage)
    {

        /*if (health > maxHealth)
        {
            health -= damage;
            
        }
        if(health <= 0) { Destroy(gameObject); }*/
        if (shield > 0) {
            Debug.Log("Player Health: " + health);
            
            Debug.Log("Player Shield: " + shield);
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
            Destroy(gameObject);
        }
    }
}