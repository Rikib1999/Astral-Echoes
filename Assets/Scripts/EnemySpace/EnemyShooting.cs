using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class ShootingEnemie : NetworkBehaviour
{
    public GameObject bullet;
    public Transform firePoint;
    public float shootingTime;

    private float timeBtwShots;
    private GameObject targetPlayer;
    private BasicEnemies chaseEnemy;

    void Start()
    {
        chaseEnemy = GetComponent<BasicEnemies>();
    }

    void Update()
    {
        FindClosestPlayer();
        if (!targetPlayer || !IsServer) return;
 
        float distance = Vector2.Distance(transform.position, targetPlayer.transform.position);

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
            .FirstOrDefault(player => Vector3.Distance(transform.position, player.position) <= chaseEnemy.chaseDistance)?.gameObject;
    }

    void shoot()
    {
        var bul = Instantiate(bullet, firePoint.position, Quaternion.identity);
        bul.GetComponent<NetworkObject>().Spawn();
    }   
}