using Assets.Scripts.PlanetResources;
using UnityEngine;
using Unity.Netcode;

//Script for the bullet that damages
public class Bullet : NetworkBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] int bulletDamage = 20;
    [SerializeField] string enemyTag;
    [SerializeField] string resourceTag;
    public float timer;

    public Vector2 bulletDirection = Vector2.right;

    private void Awake()
    {
        /*Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player.localScale.x < 0)
            bulletDirection = Vector2.left;
        else
            bulletDirection = Vector2.right;
            */
    }
    //Bullet movement
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right * bulletDirection);

        timer += Time.deltaTime;

        if (timer >= 10)
        {
            Destroy(gameObject);
        }
    }


/*    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }*/
    
    //Bullet collision effect
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!IsServer){return;}

        if (collision.gameObject.CompareTag(enemyTag))
        {
            collision.gameObject.GetComponent<EnemyLogic>().damage(bulletDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag(resourceTag))
        {
            collision.gameObject.GetComponent<ResourceDrop>().Damage(bulletDamage);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsServer){return;}

        if (collision.gameObject.CompareTag(enemyTag))
        {
            collision.gameObject.GetComponent<EnemyLogic>().damage(bulletDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag(resourceTag))
        {
            collision.gameObject.GetComponent<ResourceDrop>().Damage(bulletDamage);
            Destroy(gameObject);
        }
    }
}