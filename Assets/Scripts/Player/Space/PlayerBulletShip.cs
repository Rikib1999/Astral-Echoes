using Unity.Netcode;
using UnityEngine;

public class PlayerBulletShip : NetworkBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] int bulletDamage = 20;
    [SerializeField] string enemyTag;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        { 
            collision.gameObject.GetComponent<EnemyShipHealth>().damage(bulletDamage);
            Destroy(gameObject);
        }
    }
}