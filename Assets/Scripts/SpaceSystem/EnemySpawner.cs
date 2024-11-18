using Assets.Scripts;
using Assets.Scripts.SpaceSystem;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;

    private void Start()
    {
        SystemDataBag systemDataBag = SystemMapManager.Instance.gameObject.GetComponent<SystemDataBag>();


        float maxOrbit = 0.0f; //systemDataBag.SatelliteObjects.Max(x => x.OrbitRadius);
        
        foreach(SpaceObjectDataBag satObject in systemDataBag.SatelliteObjects)
        {
            maxOrbit = Mathf.Max(maxOrbit, satObject.OrbitRadius);
        }

        int count = Random.Range(10, 30);

        for (int i = 0; i < count; i++)
        {
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], new Vector3(Random.Range(3, maxOrbit) * SystemMapManager.scaleUpConst, Random.Range(3, maxOrbit) * SystemMapManager.scaleUpConst, -5), Quaternion.identity);
        }
    }
}