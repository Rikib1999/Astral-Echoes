using UnityEngine;
using Unity.Netcode;
public class EnemyBulletScript : NetworkBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;

    public float force = 5;
    public float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;

        //fly towards position
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        //rotate towards flying position
        transform.rotation = Quaternion.Euler(0, 0, rot+90);
    }

    void Update()
    {
        if(!IsServer){return;}
        
        timer += Time.deltaTime;

        if (timer >= 6)
        {
            Destroy(gameObject);
        }
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
