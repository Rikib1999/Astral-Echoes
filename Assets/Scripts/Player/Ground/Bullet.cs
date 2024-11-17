using Assets.Scripts.PlanetResources;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] int bulletDamage = 0;
    [SerializeField] string enemyTag;
    [SerializeField] string resourceTag;

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

    private void OnBecameInvisible()
    {
        Destroy(gameObject); // Destroy the bullet when it goes off-screen
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            collision.gameObject.GetComponent<EnemyLogic>().damage(bulletDamage); // Deal damage to the enemy
        }
        else if (collision.gameObject.CompareTag(resourceTag))
        {
            collision.gameObject.GetComponent<ResourceDrop>().Damage(bulletDamage); // Deal damage to the enemy
        }
        Destroy(gameObject); // Destroy the bullet on collision
    }
}
