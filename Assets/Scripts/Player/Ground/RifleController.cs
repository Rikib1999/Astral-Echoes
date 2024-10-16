using UnityEngine;
using Unity.Netcode;

public class RifleController : NetworkBehaviour
{
    [SerializeField] NetworkObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 0.1f;
    private float nextFireTime = 0f;
    [SerializeField] AudioSource audioSource;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();

            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        audioSource.Play();
        if (IsServer){
            var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab,NetworkManager.ServerClientId,true,false,true,firePoint.position,firePoint.rotation);
        }else{
            SpawnBulletServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnBulletServerRpc(ServerRpcParams serverRpcParams = default){
        var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab,serverRpcParams.Receive.SenderClientId,true,false,true,firePoint.position,firePoint.rotation);
    }
}
