using Assets.Scripts;
using Assets.Scripts.Resources;
using Assets.Scripts.SpaceObjects;
using Assets.Scripts.SpaceSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : ChunkGenerator<MapChunk>
{
    private const int systemChunkSize = 1024; // Defines the size of a system chunk
    private const int maxSystemLocationRetries = 3; // Maximum attempts to place a system in a chunk
    private const int blackHoleChance = 20; // Percentage chance for a black hole instead of a star
    private const int gasGiantChance = 2; // Percentage chance for a gas giant instead of a planet
    private const int minPlanetsDist = 20; // Minimum distance between planets in a system

    private float minSystemDist; // Minimum distance between two systems
    private float maxSystemRadius; // Maximum radius of the objects in a system

    private Vector2 playerPos; // Player's current position
    private int seed;

    // Chunk generator parameters
    protected override int ChunkSize { get; set; } = 2048; // Size of each map chunk
    protected override int MaxExistingChunks { get; set; } = 4; // Maximum number of loaded chunks
    protected override int VisibleCoordsDistance { get; set; } = 2; // Visibility radius in chunk coordinates

    // Prefabs for the different space objects
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private GameObject gasGiantPrefab;

    public GameObject deathAlert; // UI element for showing a death alert

    private new void Start()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Initialize player position from PlayerPrefs
        player.position = new(PlayerPrefs.GetFloat("currentSystemPositionX", 0), PlayerPrefs.GetFloat("currentSystemPositionY", 0), -10);
        playerPos = player.position;

        seed = PlayerPrefs.GetInt("seed", 0);

        base.Start(); // Call base class Start method

        // Calculate minimum system distance and max system radius based on chunk size
        minSystemDist = (systemChunkSize / 2) / Mathf.Sqrt(2);
        maxSystemRadius = minSystemDist / 2;

        // Check if a death occurred and manage the alert visibility
        if (deathAlert != null)
        {
            if (SystemMapManager.Instance.Death)
            {
                SystemMapManager.Instance.Death = false;
                deathAlert.SetActive(true); // Show death alert if needed
            }
            else
            {
                deathAlert.SetActive(false); // Hide death alert
            }
        }
    }

    public void SetSeedAndPlayerPosition()
    {
        seed = PlayerPrefs.GetInt("seed", 0);
        player.position = new(PlayerPrefs.GetFloat("currentSystemPositionX", 0), PlayerPrefs.GetFloat("currentSystemPositionY", 0), -10);
        playerPos = player.position;
    }

    private void OnSceneLoaded(Scene current, LoadSceneMode loadSceneMode)
    {
        // Reset satellite objects and central object when entering the SpaceMap scene
        if (current.name != "SpaceMap") return;

        if (SystemMapManager.Instance != null && SystemMapManager.Instance.SatelliteObjects != null) SystemMapManager.Instance.SatelliteObjects.Clear();
        if (SystemMapManager.Instance != null && SystemMapManager.Instance.CentralObject != null) SystemMapManager.Instance.CentralObject.Value = default;
    }

    protected override void DeleteChunk(MapChunk chunk)
    {
        // Safely delete all objects in the chunk
        if (chunk.SpaceObjects == null) return;

        foreach (var obj in chunk.SpaceObjects) Destroy(obj);

        chunk.SpaceObjects = null; // Clear reference to objects
    }

    protected override MapChunk GenerateChunk(Vector2Int coords)
    {
        MapChunk chunk = new() { Coords = coords }; // Create a new chunk
        ChunkDimensions chunkDims = new(ChunkSize, ChunkRadius - minSystemDist, coords); // Define chunk bounds

        // Initialize random seed based on chunk coordinates for consistent generation
        Random.InitState((int)((chunkDims.centre.x * chunkDims.centre.x + chunkDims.centre.y * chunkDims.centre.y) / Mathf.Max(chunkDims.centre.x, 1) + chunkDims.centre.y));
        Random.InitState(Random.Range(0, int.MaxValue)); // Additional shuffle for randomness
        Random.InitState(Random.Range(seed, int.MaxValue));

        List<Vector2> points = new(); // List to store generated system positions
        int retries = 0; // Counter for retry attempts

        chunk.SpaceObjects = new List<GameObject>(); // Initialize space objects in the chunk

        // Try to generate systems in the chunk
        while (retries < maxSystemLocationRetries)
        {
            Vector2 newPoint = new(Random.Range(chunkDims.left, chunkDims.right), Random.Range(chunkDims.bottom, chunkDims.top)); // Random position
            bool failed = false;

            // Ensure newPoint is not too close to existing points
            foreach (Vector2 p in points)
            {
                if (Vector2.Distance(newPoint, p) < minSystemDist)
                {
                    retries++; // Increment retries if point placement fails
                    failed = true;
                    break;
                }
            }

            if (!failed)
            {
                points.Add(newPoint); // Add valid system point
                chunk.SpaceObjects.AddRange(GenerateSystem(newPoint)); // Generate system at the point
            }
        }

        return chunk;
    }

    private List<GameObject> GenerateSystem(Vector2 point)
    {
        var systemDataBag = new SystemDataBag(); // Data bag to store information about the system
        var spaceObjects = new List<GameObject>();

        // Determine if the central object is a star or a black hole
        bool isStar = Random.Range(0, int.MaxValue) % blackHoleChance != 0;

        systemDataBag.Position = point; // Set system position

        // Calculate distance from the player and check fuel requirements
        float distance = Vector2.Distance(systemDataBag.Position, playerPos);
        systemDataBag.Distance = distance;
        systemDataBag.Fuel = PlayerPrefs.GetFloat("fuel", ResourceDefaultValues.Fuel);
        systemDataBag.CanTravel = systemDataBag.Fuel >= distance;

        // Instantiate central object (star or black hole)
        var centralObjectPrefab = isStar ? starPrefab : blackHolePrefab;
        var centralObject = Instantiate(centralObjectPrefab, new Vector3(point.x, point.y, 0), Quaternion.identity);
        spaceObjects.Add(centralObject);

        // Configure the central object
        if (isStar)
        {
            var star = centralObject.GetComponent<Star>();
            star.Randomize();
            systemDataBag.CentralObject = new SpaceObjectDataBag()
            {
                Name = star.Name,
                OrbitRadius = 0,
                Coordinates = star.transform.position,
                RelativePosition = Vector2.zero,
                Size = star.Size,
                SubType = (int)star.SubType,
                Type = star.Type
            };

            star.AddComponent<SpaceSystemClick>(); // Add interaction component
            star.GetComponent<SpaceSystemClick>().systemDataBag = systemDataBag; // Assign system data
            star.SetTooltipDistance(systemDataBag.CanTravel, distance); // Update tooltip with travel info
        }
        else
        {
            var blackHole = centralObject.GetComponent<BlackHole>();
            blackHole.Randomize();
            systemDataBag.CentralObject = new SpaceObjectDataBag()
            {
                Name = blackHole.Name,
                OrbitRadius = 0,
                Coordinates = blackHole.transform.position,
                RelativePosition = Vector2.zero,
                Size = blackHole.Size,
                SubType = (int)blackHole.SubType,
                Type = blackHole.Type
            };

            blackHole.AddComponent<SpaceSystemClick>(); // Add interaction component
            blackHole.GetComponent<SpaceSystemClick>().systemDataBag = systemDataBag; // Assign system data
            blackHole.SetTooltipDistance(systemDataBag.CanTravel, distance); // Update tooltip with travel info
        }

        float currentDist = 0;

        // Generate satellites (planets and gas giants) orbiting the central object
        while (true)
        {
            currentDist += Random.Range(minPlanetsDist, Random.Range(minPlanetsDist, maxSystemRadius)); // Randomize distance
            if (currentDist > maxSystemRadius) break; // Stop if orbit exceeds max radius

            Vector2 orbitLocation = Random.insideUnitCircle.normalized * currentDist; // Calculate orbit position

            // Determine if satellite is a planet or a gas giant
            var satelliteObjectPrefab = planetPrefab;
            bool isPlanet = currentDist <= maxSystemRadius / 2 || Random.Range(0, int.MaxValue) % gasGiantChance != 0;

            if (!isPlanet) satelliteObjectPrefab = gasGiantPrefab;

            var satelliteObject = Instantiate(satelliteObjectPrefab, new Vector3(point.x + orbitLocation.x, point.y + orbitLocation.y, 0), Quaternion.identity);
            spaceObjects.Add(satelliteObject);

            // Configure the satellite object
            if (isPlanet)
            {
                var planet = satelliteObject.GetComponent<Planet>();
                planet.Randomize();
                planet.SetOrbit(point, currentDist);
                planet.SetTooltip();

                systemDataBag.SatelliteObjects.Add(new SpaceObjectDataBag()
                {
                    Name = planet.Name,
                    OrbitRadius = currentDist,
                    Coordinates = planet.transform.position,
                    RelativePosition = planet.transform.position - centralObject.transform.position,
                    Size = planet.Size,
                    SubType = (int)planet.SubType,
                    Type = planet.Type
                });

                planet.AddComponent<SpaceSystemClick>(); // Add interaction component
                planet.GetComponent<SpaceSystemClick>().systemDataBag = systemDataBag; // Assign system data
                planet.SetTooltipDistance(systemDataBag.CanTravel, distance); // Update tooltip with travel info
            }
            else
            {
                var gasGiant = satelliteObject.GetComponent<GasGiant>();
                gasGiant.Randomize();
                gasGiant.SetOrbit(point, currentDist);
                gasGiant.SetTooltip();

                systemDataBag.SatelliteObjects.Add(new SpaceObjectDataBag()
                {
                    Name = gasGiant.Name,
                    OrbitRadius = currentDist,
                    Coordinates = gasGiant.transform.position,
                    RelativePosition = gasGiant.transform.position - centralObject.transform.position,
                    Size = gasGiant.Size,
                    SubType = (int)gasGiant.SubType,
                    Type = gasGiant.Type
                });

                gasGiant.AddComponent<SpaceSystemClick>(); // Add interaction component
                gasGiant.GetComponent<SpaceSystemClick>().systemDataBag = systemDataBag; // Assign system data
                gasGiant.SetTooltipDistance(systemDataBag.CanTravel, distance); // Update tooltip with travel info
            }
        }

        return spaceObjects; // Return all generated objects in the system
    }
}