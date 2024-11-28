using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemie : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;
    public float shootingTime;


    private float timeBtwShots;
    private GameObject player;
    private BasicEnemies chaseEnemy;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        chaseEnemy = GetComponent<BasicEnemies>();

    }

    // Update is called once per frame
    void Update()
    {
        //Fix for error when player = null
        if(!player)
        {
            return;
        }
 
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if(distance < chaseEnemy.chaseDistance )
        {

            timeBtwShots += Time.deltaTime;

            Vector2 hm = transform.localScale;

            float fasterBullets = hm.x;

            //Debug.Log(hm.x);

            shootingTime = fasterBullets - 0.2f;

            if (timeBtwShots >= shootingTime)
            {
                //Instantiate(bullet, firePoint.position, firePoint.rotation);
                timeBtwShots = 0;
                shoot();
            }

        }



        
    }

    void shoot()
    {
        Instantiate(bullet, firePoint.position, Quaternion.identity);
    }   
}
