using Assets.Scripts;
using Assets.Scripts.SpaceObjects;
using Assets.Scripts.SpaceSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class MapGenerator : ChunkGenerator<MapChunk>
{
    private const int systemChunkSize = 1024;
    private const int maxSystemLocationRetries = 3;
    private const int blackHoleChance = 20;
    private const int gasGiantChance = 2;
    private const int minPlanetsDist = 20;

    private float minSystemDist;
    private float maxSystemRadius;

    protected override int ChunkSize { get; set; } = 2048;
    protected override int MaxExistingChunks { get; set; } = 4;
    protected override int VisibleCoordsDistance { get; set; } = 2;

    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private GameObject gasGiantPrefab;

    private new void Start()
	{
        if(!IsServer){
            enabled=false;
            return;
        }
                    
		base.Start();

        minSystemDist = (systemChunkSize / 2) / Mathf.Sqrt(2);
        maxSystemRadius = minSystemDist / 2;
    }

	protected override void DeleteChunk(MapChunk chunk)
	{
        if (chunk.SpaceObjects == null) return;

        foreach (var obj in chunk.SpaceObjects) Destroy(obj);

        chunk.SpaceObjects = null;
    }

    protected override MapChunk GenerateChunk(Vector2Int coords)
    {
        MapChunk chunk = new() { Coords = coords };
        ChunkDimensions chunkDims = new(ChunkSize, ChunkRadius - minSystemDist, coords);

        Random.InitState((int)((chunkDims.centre.x * chunkDims.centre.x + chunkDims.centre.y * chunkDims.centre.y) / Mathf.Max(chunkDims.centre.x, 1) + chunkDims.centre.y));
        Random.InitState(Random.Range(0, int.MaxValue));

        List<Vector2> points = new();
        int retries = 0;

        chunk.SpaceObjects = new List<GameObject>();

        while (retries < maxSystemLocationRetries)
        {
            Vector2 newPoint = new(Random.Range(chunkDims.left, chunkDims.right), Random.Range(chunkDims.bottom, chunkDims.top));
            bool failed = false;

            foreach (Vector2 p in points)
            {
                if (Vector2.Distance(newPoint, p) < minSystemDist)
                {
                    retries++;
                    failed = true;
                    break;
                }
            }

            if (!failed)
            {
                points.Add(newPoint);
                chunk.SpaceObjects.AddRange(GenerateSystem(newPoint));
            }
        }

        return chunk;
    }

    private List<GameObject> GenerateSystem(Vector2 point)
    {
        //SystemDataBag systemDataBagRef;//  = new SystemDataBag();
        //systemDataBag.CentralObject = new NetworkVariable<SpaceObjectDataBag>();
        //systemDataBag.SatelliteObjects = new NetworkList<SpaceObjectDataBag>();
        var spaceObjects = new List<GameObject>();

        bool isStar = Random.Range(0, int.MaxValue) % blackHoleChance != 0;

        var centralObjectPrefab = isStar ? starPrefab : blackHolePrefab;
        var centralObject = NetworkManager.Instantiate(centralObjectPrefab, new Vector3(point.x, point.y, 0), Quaternion.identity);
        spaceObjects.Add(centralObject);

        //SpaceSystemClick clickComponent;
        SpaceObjectDataBag bagRef;
        if (isStar)
        {
            var star = centralObject.GetComponent<Star>();
            star.Randomize();

            star.AddComponent<SpaceSystemClick>();
            //clickComponent = star.GetComponent<SpaceSystemClick>();
            
            bagRef = new SpaceObjectDataBag()
            {
                Name = star.Name,
                OrbitRadius = 0,
                Coordinates = star.transform.position,
                RelativePosition = Vector2.zero,
                Size = star.Size,
                SubType = (int)star.SubType,
                Type = star.Type
            };
        }
        else
        {
            var blackHole = centralObject.GetComponent<BlackHole>();
            blackHole.Randomize();

            blackHole.AddComponent<SpaceSystemClick>();
            //clickComponent = blackHole.GetComponent<SpaceSystemClick>();
            
            bagRef = new SpaceObjectDataBag()
            {
                Name = blackHole.Name,
                OrbitRadius = 0,
                Coordinates = blackHole.transform.position,
                RelativePosition = Vector2.zero,
                Size = blackHole.Size,
                SubType = (int)blackHole.SubType,
                Type = blackHole.Type
            };
        }

        //Debug.Log(JsonUtility.ToJson(bagRef, true));
        //Debug.Log(JsonUtility.ToJson(clickComponent, true));
        //Debug.Log(clickComponent);
        //centralObject.AddComponent<SystemDataBag>();
        centralObject.GetComponent<NetworkObject>().Spawn(true);
        centralObject.GetComponent<SystemDataBag>().CentralObject.Value = bagRef;

        //Debug.Log(JsonUtility.ToJson(clickComponent, true));

        float currentDist = 0;

        while (true)
        {
            currentDist += Random.Range(minPlanetsDist, Random.Range(minPlanetsDist, maxSystemRadius));
            if (currentDist > maxSystemRadius) break;

            Vector2 orbitLocation = Random.insideUnitCircle.normalized;
            orbitLocation *= currentDist;

            var satelliteObjectPrefab = planetPrefab;
            bool isPlanet = currentDist <= maxSystemRadius / 2 || Random.Range(0, int.MaxValue) % gasGiantChance != 0;

            if (!isPlanet) satelliteObjectPrefab = gasGiantPrefab;

            var satelliteObject = Instantiate(satelliteObjectPrefab, new Vector3(point.x + orbitLocation.x, point.y + orbitLocation.y, 0), Quaternion.identity);
            spaceObjects.Add(satelliteObject);

            if (isPlanet)
            {
                var planet = satelliteObject.GetComponent<Planet>();
                planet.Randomize();
                planet.SetOrbit(point, currentDist);
                planet.SetTooltip();

                centralObject.GetComponent<SystemDataBag>().SatelliteObjects.Add(new SpaceObjectDataBag()
                {
                    Name = planet.Name,
                    OrbitRadius = currentDist,
                    Coordinates = planet.transform.position,
                    RelativePosition = planet.transform.position - centralObject.transform.position,
                    Size = planet.Size,
                    SubType = (int)planet.SubType,
                    Type = planet.Type
                });

                planet.AddComponent<SpaceSystemClick>();
                //planet.AddComponent<SystemDataBag>();
                planet.GetComponent<NetworkObject>().Spawn(true);
                planet.GetComponent<SystemDataBag>().CentralObject.Value = centralObject.GetComponent<SystemDataBag>().CentralObject.Value;
                //planet.GetComponent<SystemDataBag>().SatelliteObjects.Value = centralObject.GetComponent<SystemDataBag>().SatelliteObjects.Value;
                foreach(SpaceObjectDataBag satObject in centralObject.GetComponent<SystemDataBag>().SatelliteObjects)
                {
                    planet.GetComponent<SystemDataBag>().SatelliteObjects.Add(satObject);
                }
            }
            else
            {
                var gasGiant = satelliteObject.GetComponent<GasGiant>();
                gasGiant.Randomize();
                gasGiant.SetOrbit(point, currentDist);
                gasGiant.SetTooltip();

                centralObject.GetComponent<SystemDataBag>().SatelliteObjects.Add(new SpaceObjectDataBag()
                {
                    Name = gasGiant.Name,
                    OrbitRadius = currentDist,
                    Coordinates = gasGiant.transform.position,
                    RelativePosition = gasGiant.transform.position - centralObject.transform.position,
                    Size = gasGiant.Size,
                    SubType = (int)gasGiant.SubType,
                    Type = gasGiant.Type
                });

                gasGiant.AddComponent<SpaceSystemClick>();
                //gasGiant.AddComponent<SystemDataBag>();
                gasGiant.GetComponent<NetworkObject>().Spawn(true);
                gasGiant.GetComponent<SystemDataBag>().CentralObject.Value = centralObject.GetComponent<SystemDataBag>().CentralObject.Value;
                //gasGiant.GetComponent<SystemDataBag>().SatelliteObjects.Value = centralObject.GetComponent<SystemDataBag>().SatelliteObjects.Value;
                foreach(SpaceObjectDataBag satObject in centralObject.GetComponent<SystemDataBag>().SatelliteObjects)
                {
                    gasGiant.GetComponent<SystemDataBag>().SatelliteObjects.Add(satObject);
                }
            }
        }

        return spaceObjects;
    }
}