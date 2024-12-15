using Unity.Netcode;
using UnityEngine;

//Script for the player shooting by simpling spawning a bullet and moving it in direction player is shooting at 
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
        if(!IsOwner){return;}

        if (Time.time >= nextFireTime)
        {
            audioSource.Play();
                var cntrl = gameObject.GetComponentInParent<Controller>();

            if (IsServer)
            {
                var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, NetworkManager.ServerClientId, true, false, true, firePoint.position, firePoint.rotation);
                if(cntrl && !cntrl.facingRight) playerNetworkObject.GetComponent<Bullet>().bulletDirection = Vector2.left;
            }
            else
            {
                SpawnBulletServerRpc(firePoint.position,firePoint.rotation,cntrl && !cntrl.facingRight);
            }

            nextFireTime = Time.time + pistolFireRate;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnBulletServerRpc(Vector3 pos, Quaternion rot, bool flip, ServerRpcParams serverRpcParams = default)
    {

        //var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, serverRpcParams.Receive.SenderClientId, true, false, true, pos, Quaternion.Euler(rot2));
        var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, serverRpcParams.Receive.SenderClientId, true, false, true, pos, rot);
        if(flip) playerNetworkObject.GetComponent<Bullet>().bulletDirection = Vector2.left;
        
    }
}