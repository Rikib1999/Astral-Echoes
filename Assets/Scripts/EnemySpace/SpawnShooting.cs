using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnShooting : NetworkBehaviour
{
    public GameObject ShipToSpawn;
    public Transform SpawnPoint;
    


    //private float TimeBetweenSpawns;
    //private GameObject player;
    //private MotherEnemy motheren;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");

        //motheren = GetComponent<MotherEnemy>();

    }

    // Update is called once per frame
    void Update()
    {



        /*float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < motheren.StopToSpawnDistance)
        {

            TimeBetweenSpawns += Time.deltaTime;

            Vector2 hm = transform.localScale;

            float fasterBullets = hm.x;

            //Debug.Log(hm.x);

            SpawnTime = fasterBullets - 0.2f;

            if (TimeBetweenSpawns >= SpawnTime)
            {
                //Instantiate(bullet, firePoint.position, firePoint.rotation);
                TimeBetweenSpawns = 0;
                shoot();
            }

        }*/




    }
    /// <summary>
    /// Spawn enemy ship that will shoot at the player
    /// </summary>
    public void SpawnShip()
    {
        if(!IsServer)
        {
            return;
        }

        Debug.Log("Spawned");

        var ship = Instantiate(ShipToSpawn, SpawnPoint.position, Quaternion.identity);
        ship.GetComponent<NetworkObject>().Spawn();

    }
}
