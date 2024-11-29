using UnityEngine;

public class EnemyBulletPattern : MonoBehaviour
{
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 6)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetDamaged(other.gameObject);
            Destroy(gameObject);
        }
    }

    void GetDamaged(GameObject player)
    {
        ShipHealth shipHealth = player.GetComponent<ShipHealth>();
        
        if( shipHealth != null)
        {
            shipHealth.damage(10);
        }
    }
}