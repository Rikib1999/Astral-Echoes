using Unity.Netcode;
using UnityEngine;

public class Shotgun : NetworkBehaviour
{
    [SerializeField] NetworkObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] int numBullets = 4;
    [SerializeField] float spreadAngle = 30f;
    [SerializeField] AudioSource audioSource;
    Quaternion bulletRotation;
    private bool hasWeapon;
    private ResourceTextUpdater resourceTextUpdater;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        gameObject.SetActive(false);
    }

    private void Start()
    {
        hasWeapon = PlayerPrefs.GetInt("shotgun", 0) == 1;
        resourceTextUpdater = FindFirstObjectByType<ResourceTextUpdater>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && hasWeapon && resourceTextUpdater.ammoCount > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        audioSource.Play();

        resourceTextUpdater.ammoCount--;
        resourceTextUpdater.SetAmmo(resourceTextUpdater.ammoCount);

        for (int i = 0; i < numBullets; i++)
        {
            float randomAngle = Random.Range(-spreadAngle, spreadAngle);
            bulletRotation = Quaternion.Euler(0f, 0f, firePoint.rotation.eulerAngles.z + randomAngle);

            if (IsServer)
            {
                var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, NetworkManager.ServerClientId, true, false, true, firePoint.position, bulletRotation);
            }
            else
            {
                SpawnBulletServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnBulletServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, serverRpcParams.Receive.SenderClientId, true, false, true, firePoint.position, bulletRotation);
    }
}