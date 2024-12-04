using Unity.Netcode;
using UnityEngine;

public class PlayerBulletShip : NetworkBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] int bulletDamage = 20;
    [SerializeField] string enemyTag;
    public float timer;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsServer){return;}

        if (collision.gameObject.CompareTag(enemyTag))
        { 
            collision.gameObject.GetComponent<EnemyShipHealth>().damage(bulletDamage);
            Destroy(gameObject);
        }
    }
}