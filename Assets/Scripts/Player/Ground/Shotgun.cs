using Unity.Netcode;
using UnityEngine;

//Script for the Shotgun controller -> Shoots in different angles 
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
        if(!IsOwner){return;}

        if (Input.GetButtonDown("Fire1") && hasWeapon && resourceTextUpdater.ammoCount > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        audioSource.Play();
                var cntrl = gameObject.GetComponentInParent<Controller>();

        resourceTextUpdater.ammoCount--;
        resourceTextUpdater.SetAmmo(resourceTextUpdater.ammoCount);

        for (int i = 0; i < numBullets; i++)
        {
            float randomAngle = Random.Range(-spreadAngle, spreadAngle);
            bulletRotation = Quaternion.Euler(0f, 0f, firePoint.rotation.eulerAngles.z + randomAngle);

            if (IsServer)
            {
                var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, NetworkManager.ServerClientId, true, false, true, firePoint.position, bulletRotation);
                if(cntrl && !cntrl.facingRight) playerNetworkObject.GetComponent<Bullet>().bulletDirection = Vector2.left;
            }
            else
            {
                SpawnBulletServerRpc(firePoint.position,firePoint.rotation,cntrl && !cntrl.facingRight);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnBulletServerRpc(Vector3 pos, Quaternion rot, bool flip, ServerRpcParams serverRpcParams = default)
    {
        var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, serverRpcParams.Receive.SenderClientId, true, false, true, firePoint.position, bulletRotation);
        if(flip) playerNetworkObject.GetComponent<Bullet>().bulletDirection = Vector2.left;
    }
}