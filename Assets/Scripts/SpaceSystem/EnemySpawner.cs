using Assets.Scripts;
using Assets.Scripts.SpaceSystem;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;

    private void Start()
    {
        if(!IsServer)
        {
            enabled=false;
            return;
        }
        float maxOrbit = 0.0f; //systemDataBag.SatelliteObjects.Max(x => x.OrbitRadius);
        
        foreach(SpaceObjectDataBag satObject in SystemMapManager.Instance.SatelliteObjects)
        {
            maxOrbit = Mathf.Max(maxOrbit, satObject.OrbitRadius);
        }

        int count = Random.Range(30, 50);

        for (int i = 0; i < count; i++)
        {
            int signX = Random.Range(0, 2) > 0 ? 1 : -1;
            int signY = Random.Range(0, 2) > 0 ? 1 : -1;

            var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], new Vector3(Random.Range(3, maxOrbit) * SystemMapManager.scaleUpConst * signX, Random.Range(3, maxOrbit) * SystemMapManager.scaleUpConst * signY, -5), Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn(true);
        }
    }
}