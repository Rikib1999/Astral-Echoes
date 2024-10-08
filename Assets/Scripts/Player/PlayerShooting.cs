using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerShooting : NetworkBehaviour
{
    public NetworkObject bullet;
    public Transform firePoint;
    public float shootingTime;

    public GameObject crosshair;




    // Start is called before the first frame update
    void Start()
    {
        if(!IsOwner) //Disable this script if not owner
        {
            enabled=false;
        }else{
            crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            firePoint = gameObject.transform.Find("FirePosPlayer");
        }

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
        //Instantiate(bullet, firePoint.position, Quaternion.identity);
        if(IsServer){
            var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bullet,NetworkManager.ServerClientId,true,false,true,firePoint.position,Quaternion.identity);
            playerNetworkObject.GetComponent<PlayerBulletControl>().crosshair_pos = crosshair.transform.position;
        }else{
            SpawnBulletServerRpc(firePoint.position,crosshair.transform.position);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnBulletServerRpc(Vector3 bullet_pos,Vector3 target,ServerRpcParams serverRpcParams = default){
        var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bullet,serverRpcParams.Receive.SenderClientId,true,false,true,bullet_pos,Quaternion.identity);
        playerNetworkObject.GetComponent<PlayerBulletControl>().crosshair_pos = target;
    }



}
