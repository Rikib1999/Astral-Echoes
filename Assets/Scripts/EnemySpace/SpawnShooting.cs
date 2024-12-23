using Unity.Netcode;
using UnityEngine;

public class SpawnShooting : NetworkBehaviour
{
    public GameObject ShipToSpawn;
    public Transform SpawnPoint;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Spawn enemy ship that will shoot at the player
    /// </summary>
    public void SpawnShip()
    {
        if(!IsServer)
        {
            return;
        }

        Debug.Log("Spawned");

        var ship = Instantiate(ShipToSpawn, SpawnPoint.position, Quaternion.identity);
        ship.GetComponent<NetworkObject>().Spawn();

    }
}
