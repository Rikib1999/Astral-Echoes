using Assets.Scripts;
using Assets.Scripts.SpaceObjects;
using System.Collections.Generic;
using UnityEngine;

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
        var spaceObjects = new List<GameObject>();

        var centralObjectPrefab = Random.Range(0, int.MaxValue) % blackHoleChance == 0 ? blackHolePrefab : starPrefab;
        var centralObject = Instantiate(centralObjectPrefab, new Vector3(point.x, point.y, 0), Quaternion.identity);
        spaceObjects.Add(centralObject);

        float currentDist = 0;

        while (true)
        {
            currentDist += Random.Range(minPlanetsDist, Random.Range(minPlanetsDist, maxSystemRadius));
            if (currentDist > maxSystemRadius) break;

            Vector2 orbitLocation = Random.insideUnitCircle.normalized;
            orbitLocation *= currentDist;

            var satelliteObjectPrefab = planetPrefab;

            if (currentDist > maxSystemRadius / 2 && Random.Range(0, int.MaxValue) % gasGiantChance == 0) satelliteObjectPrefab = gasGiantPrefab;

            var planet = Instantiate(satelliteObjectPrefab, new Vector3(point.x + orbitLocation.x, point.y + orbitLocation.y, 0), Quaternion.identity);
            planet.GetComponent<SatelliteObject>().SetOrbit(point, currentDist);

            spaceObjects.Add(planet);
        }

        return spaceObjects;
    }
}