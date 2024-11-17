using Assets.Scripts.PlanetResources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletControl : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float speed = 10f;
    [SerializeField] int bulletDamage = 0;
    [SerializeField] string enemyTag;

    private Vector2 bulletDirection; // Store the initial direction of the bullet

    private void Awake()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        // Calculate the initial direction based on the player's facing direction
        if (player.localScale.x < 0)
            bulletDirection = Vector2.left;  // If player is facing left
        else
            bulletDirection = Vector2.right; // If player is facing right
    }

    void Update()
    {
        // Move the bullet in the initial direction
        transform.Translate(bulletDirection * speed * Time.deltaTime);
    }


    // Detect collision with an enemy
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GetDamage(other.gameObject); // Apply damage to the enemy
            Destroy(gameObject); // Destroy the bullet
        }
        else if (other.gameObject.CompareTag("PlanetResource"))
        {
            other.gameObject.GetComponent<ResourceDrop>().Damage(bulletDamage); // Deal damage to the enemy
            Destroy(gameObject); // Destroy the bullet
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject); // Destroy the bullet when it goes off-screen
    }

    // Apply damage to the enemy when hit
    void GetDamage(GameObject enemy)
    {
        EnemyShipHealth eshipHealth = enemy.GetComponent<EnemyShipHealth>();

        if (eshipHealth != null)
        {
            eshipHealth.damage(5); // Apply damage (5 points in this case)
        }
        else
        {
            Debug.Log("EnemyShipHealth is null");
        }
    }
}
