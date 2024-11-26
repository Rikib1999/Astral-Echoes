using Assets.Scripts;
using Assets.Scripts.SpaceSystem;
using System.Linq;
using UnityEngine;
using Unity.Netcode;

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

        int count = Random.Range(10, 30);

        for (int i = 0; i < count; i++)
        {
            var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], new Vector3(Random.Range(3, maxOrbit) * SystemMapManager.scaleUpConst, Random.Range(3, maxOrbit) * SystemMapManager.scaleUpConst, -5), Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn(true);

        }
    }
}