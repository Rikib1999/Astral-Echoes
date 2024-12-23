using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class EnemyBulletScript : NetworkBehaviour
{
    private GameObject targetPlayer;
    private Rigidbody2D rb;

    public float force = 5;
    public float timer;

    void Start()
    {
        FindClosestPlayer();
        rb = GetComponent<Rigidbody2D>();

        Vector3 direction = targetPlayer.transform.position - transform.position;

        //fly towards position
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        //rotate towards flying position
        transform.rotation = Quaternion.Euler(0, 0, rot+90);
    }

    void Update()
    {
        if (!IsServer){return;}
        
        timer += Time.deltaTime;

        if (timer >= 6)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Finds the closest player within detection range.
    /// </summary>
    private void FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
        {
            targetPlayer = null;
            return;
        }

        targetPlayer = players
            .Select(player => player.transform)
            .OrderBy(player => Vector3.Distance(transform.position, player.position))
            .FirstOrDefault()?.gameObject;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //if collide with player trigger damage and destroy itself
        if (other.gameObject.CompareTag("Player"))
        {
            GetDamaged(other.gameObject);
            Destroy(gameObject);
        }
        Debug.Log("Hit");
    }

    /// <summary>
    /// Get the player's ship health and damage it
    /// </summary>
    /// <param name="player"></param>
    void GetDamaged(GameObject player)
    {
        ShipHealth shipHealth = player.GetComponent<ShipHealth>();
        
        if( shipHealth != null)
        {
            shipHealth.damage(10);
        }
        else
        {
            Debug.Log("PlayerLogic is null");
        }
    }
}
