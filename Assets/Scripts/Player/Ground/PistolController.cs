using Unity.Netcode;
using UnityEngine;

public class PistolController : NetworkBehaviour
{
    [SerializeField] NetworkObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float pistolFireRate = 0.5f;
    [SerializeField] AudioSource audioSource;

    private float nextFireTime = 0f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootPistol();
        }
    }

    void ShootPistol()
    {
        if (Time.time >= nextFireTime)
        {
            audioSource.Play();

            if (IsServer)
            {
                var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, NetworkManager.ServerClientId, true, false, true, firePoint.position, firePoint.rotation);
            }
            else
            {
                SpawnBulletServerRpc();
            }

            nextFireTime = Time.time + pistolFireRate;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnBulletServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, serverRpcParams.Receive.SenderClientId, true, false, true, firePoint.position, firePoint.rotation);
    }
}