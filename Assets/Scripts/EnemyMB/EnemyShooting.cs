using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemie : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;

    private float timeBtwShots;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

 

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if(distance < 10 )
        {

            timeBtwShots += Time.deltaTime;

            if (timeBtwShots >= 1)
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
