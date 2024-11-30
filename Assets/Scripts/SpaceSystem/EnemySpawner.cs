using Assets.Scripts;
using Assets.Scripts.SpaceSystem;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    private List<GameObject> spawnedEnemies = new();

    private void Start()
    {
        if (!IsServer)
        {
            enabled=false;
            return;
        }

        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        float maxOrbit = 0.0f;
        
        foreach (SpaceObjectDataBag satObject in SystemMapManager.Instance.SatelliteObjects)
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
            spawnedEnemies.Add(enemy);
        }
    }

    private void OnSceneUnloaded(Scene current)
    {
        if (spawnedEnemies == null) return;

        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                var networkComp = enemy.GetComponent<NetworkObject>();
                if (networkComp != null)
                {
                    networkComp.Despawn();
                    continue;
                }
                Destroy(enemy);
            }
        }
    }
}