using UnityEngine;
using Unity.Netcode;

public class ShootingEnemie : NetworkBehaviour
{
    public GameObject bullet;
    public Transform firePoint;
    public float shootingTime;

    private float timeBtwShots;
    private GameObject player;
    private BasicEnemies chaseEnemy;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        chaseEnemy = GetComponent<BasicEnemies>();
    }

    void Update()
    {
        if (!player || !IsServer) return;
 
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < chaseEnemy.chaseDistance )
        {

            timeBtwShots += Time.deltaTime;

            Vector2 hm = transform.localScale;

            float fasterBullets = hm.x;

            //Debug.Log(hm.x);

            shootingTime = fasterBullets - 0.2f;

            if (timeBtwShots >= shootingTime)
            {
                //when timer is same as shooting time or bigger, enemy can shoot
                timeBtwShots = 0;
                shoot();
            }
        }        
    }

    void shoot()
    {
        var bul = Instantiate(bullet, firePoint.position, Quaternion.identity);
        bul.GetComponent<NetworkObject>().Spawn();
    }   
}