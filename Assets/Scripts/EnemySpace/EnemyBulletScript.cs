using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
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

        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rot+90);
    }

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
        Debug.Log("Hit");
    }

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
