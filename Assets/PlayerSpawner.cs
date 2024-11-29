using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] public NetworkObject playerPrefab;
    void Start()
    {
        //var instance = Instantiate(playerPrefab);
        //var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        //instanceNetworkObject.Spawn();

        if (IsServer)
        {
            var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(playerPrefab, NetworkManager.ServerClientId, true, true);
        }
        else
        {
            SpawnPlayerServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnPlayerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(playerPrefab, serverRpcParams.Receive.SenderClientId, true, true);
    }

    /*void OnNetworkSpawn()
    {
        var playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(playerPrefab,NetworkManager.ServerClientId,true,true);
        Debug.Log("Spawned playerJoin");
    }*/
}