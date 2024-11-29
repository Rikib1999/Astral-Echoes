using Assets.Scripts.PlanetResources;
using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] int bulletDamage = 20;
    [SerializeField] string enemyTag;
    [SerializeField] string resourceTag;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        if (collision.gameObject.CompareTag(enemyTag))
        { 
            collision.gameObject.GetComponent<EnemyShipHealth>().damage(bulletDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag(resourceTag))
        {
            collision.gameObject.GetComponent<ResourceDrop>().Damage(bulletDamage);
            Destroy(gameObject);
        }
    }
}