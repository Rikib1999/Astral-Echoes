using UnityEngine;
using Unity.Netcode;
//Script for the Rifle Controller and networking of it
public class RifleController : NetworkBehaviour
{
    //Components for the shooting
    //Same working as the Gun controller with better firerate
    [SerializeField] NetworkObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 0.1f;
    private float nextFireTime = 0f;
    [SerializeField] AudioSource audioSource;
    private bool hasWeapon;
    private ResourceTextUpdater resourceTextUpdater;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        gameObject.SetActive(false);
    }

    private void Start()
    {
        hasWeapon = PlayerPrefs.GetInt("rifle", 0) == 1;
        resourceTextUpdater = FindFirstObjectByType<ResourceTextUpdater>();
    }

    void Update()
    {
        if(!IsOwner){return;}

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && hasWeapon && resourceTextUpdater.ammoCount > 0)
        {
            Shoot();

            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        audioSource.Play();
                var cntrl = gameObject.GetComponentInParent<Controller>();

        resourceTextUpdater.ammoCount--;
        resourceTextUpdater.SetAmmo(resourceTextUpdater.ammoCount);

        if (IsServer)
        {
            var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, NetworkManager.ServerClientId, true, false, true, firePoint.position, firePoint.rotation);
            if(cntrl && !cntrl.facingRight) playerNetworkObject.GetComponent<Bullet>().bulletDirection = Vector2.left;
        }
        else
        {
            SpawnBulletServerRpc(firePoint.position,firePoint.rotation,cntrl && !cntrl.facingRight);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnBulletServerRpc(Vector3 pos, Quaternion rot, bool flip, ServerRpcParams serverRpcParams = default)
    {
        var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, serverRpcParams.Receive.SenderClientId, true, false, true, firePoint.position, firePoint.rotation);
        if(flip) playerNetworkObject.GetComponent<Bullet>().bulletDirection = Vector2.left;
    }
}