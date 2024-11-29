using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    public NetworkObject bullet;
    public Transform firePoint;
    public float shootingTime;
    public AudioSource bulletSource;
    public GameObject crosshair;

    private void Start()
    {
        if (!IsOwner) //Disable this script if not owner
        { 
            enabled=false;
        }
        else
        {
            bulletSource = GetComponent<AudioSource>();
            crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        }
    }

    /*private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shoot();
            bulletSource.Play();
        }
    }*/

    private void shoot()
    {
        if (IsServer){
            var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bullet,NetworkManager.ServerClientId,true,false,true,firePoint.position,Quaternion.identity);

            Vector3 direction = (crosshair.transform.position - transform.position).normalized;
            direction.z = 0;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            playerNetworkObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            SpawnBulletServerRpc(firePoint.position,crosshair.transform.position);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBulletServerRpc(Vector3 bullet_pos,Vector3 target,ServerRpcParams serverRpcParams = default)
    {
        var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bullet,serverRpcParams.Receive.SenderClientId,true,false,true,bullet_pos,Quaternion.identity);
    }
}