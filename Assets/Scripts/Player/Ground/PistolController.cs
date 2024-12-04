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
        if(!IsOwner){return;}

        if (Time.time >= nextFireTime)
        {
            audioSource.Play();

            if (IsServer)
            {
                var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, NetworkManager.ServerClientId, true, false, true, firePoint.position, firePoint.rotation);
            }
            else
            {
                var cntrl = gameObject.GetComponentInParent<Controller>();
                SpawnBulletServerRpc(firePoint.position,firePoint.rotation,cntrl && !cntrl.facingRight);
            }

            nextFireTime = Time.time + pistolFireRate;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnBulletServerRpc(Vector3 pos, Quaternion rot, bool flip, ServerRpcParams serverRpcParams = default)
    {
        var rot2 = rot.eulerAngles;
        if(flip){
            //Debug.Log("Cleft");
		    rot2 = new Vector3(rot.x, rot.y, rot.z + 180);
        }
        var cntrl = gameObject.GetComponentInParent<Controller>();
        Debug.Log(cntrl.facingRight);
        if(cntrl && !cntrl.facingRight){
            //Debug.Log("Sleft");
            rot2 = new Vector3(rot.x, rot.y, rot.z + 180);
        }

        //var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, serverRpcParams.Receive.SenderClientId, true, false, true, pos, Quaternion.Euler(rot2));
        var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(bulletPrefab, serverRpcParams.Receive.SenderClientId, true, false, true, pos, rot);
        
    }
}