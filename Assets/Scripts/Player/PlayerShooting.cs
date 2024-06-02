using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;
    public float shootingTime;

    public GameObject crosshair;




    // Start is called before the first frame update
    void Start()
    {

        crosshair = GameObject.FindGameObjectWithTag("Crosshair"); 

    }

    // Update is called once per frame
    void Update()
    {



        float distance = Vector2.Distance(transform.position, crosshair.transform.position);



        /*timeBtwShots += Time.deltaTime;



        if (timeBtwShots >= 1)
        {
            //Instantiate(bullet, firePoint.position, firePoint.rotation);
            timeBtwShots = 0;
            shoot();
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            shoot();
        }

        




    }


    void shoot()
    {
        Instantiate(bullet, firePoint.position, Quaternion.identity);
    }



}
