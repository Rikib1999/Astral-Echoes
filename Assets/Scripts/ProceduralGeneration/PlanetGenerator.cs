using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.Planet;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlanetGenerator : ChunkGenerator<PlanetChunk>
{
    private readonly float scale = 0.5f;

    protected override int ChunkSize { get; set; } = 32;
    protected override int MaxExistingChunks { get; set; } = 32;
    protected override int VisibleCoordsDistance { get; set; } = 2;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private PlanetPalettesList planetPalettesList;
    [SerializeField] private PlanetResourcesList planetResourcesList;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private GameObject[] bushPrefabs;
    [SerializeField] private GameObject[] stonePrefabs;
    [SerializeField] private GameObject[] crystalPrefabs;

    private PlanetPaletteBag planetPaletteBag;

    private new void Start()
    {
        base.Start();

        planetPaletteBag = new(GetPlanetPalette());
        PlanetMapManager.Instance.ComputeSeed(); //Recompute the seed because the networkVariable is slow to synchronize
        SetPlanetResources();
        NoiseS3D.seed = PlanetMapManager.Seed;

        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene current)
    {
        if (chunkList == null) return;

        foreach (var chunk in chunkList)
        {
            foreach (var obj in chunk.PlanetObjects)
            {
                if (obj != null)
                {
                    var networkComp = obj.GetComponent<NetworkObject>();
                    if (networkComp != null)
                    {
                        networkComp.Despawn();
                        continue;
                    }
                    Destroy(obj);
                }
            }
        }
    }

    private PlanetPalette GetPlanetPalette()
    {
        switch ((ePlanetType)PlanetMapManager.Instance.PlanetDataBag.Value.SubType)
        {
            case ePlanetType.Airless: return planetPalettesList.airlessPlanets;
            case ePlanetType.Aquamarine: return planetPalettesList.aquamarinePlanets;
            case ePlanetType.Arid: return planetPalettesList.aridPlanets;
            case ePlanetType.Barren: return planetPalettesList.barrenPlanets;
            case ePlanetType.Cloudy: return planetPalettesList.cloudyPlanets;
            case ePlanetType.Cratered: return planetPalettesList.crateredPlanets;
            case ePlanetType.Dry: return planetPalettesList.dryPlanets;
            case ePlanetType.Frozen: return planetPalettesList.frozenPlanets;
            case ePlanetType.Glacial: return planetPalettesList.glacialPlanets;
            case ePlanetType.Icy: return planetPalettesList.icyPlanets;
            case ePlanetType.Lunar: return planetPalettesList.lunarPlanets;
            case ePlanetType.Lush: return planetPalettesList.lushPlanets;
            case ePlanetType.Magma: return planetPalettesList.magmaPlanets;
            case ePlanetType.Muddy: return planetPalettesList.muddyPlanets;
            case ePlanetType.Oasis: return planetPalettesList.oasisPlanets;
            case ePlanetType.Rocky: return planetPalettesList.rockyPlanets;
            case ePlanetType.Snowy: return planetPalettesList.snowyPlanets;
            case ePlanetType.Terrestrial: return planetPalettesList.terrestrialPlanets;
            case ePlanetType.Tropical: return planetPalettesList.tropicalPlanets;
            default: return null;
        }
    }

    private void SetPlanetResources()
    {
        treePrefabs = planetResourcesList.trees[UnityEngine.Random.Range(0, planetResourcesList.trees.Length)].resources;
        bushPrefabs = planetResourcesList.bushes[UnityEngine.Random.Range(0, planetResourcesList.bushes.Length)].resources;
        stonePrefabs = planetResourcesList.stones[UnityEngine.Random.Range(0, planetResourcesList.stones.Length)].resources;
        crystalPrefabs = planetResourcesList.crystals[UnityEngine.Random.Range(0, planetResourcesList.crystals.Length)].resources;
    }

    protected override void DeleteChunk(PlanetChunk chunk)
    {
        for (int y = -ChunkRadius; y <= ChunkRadius; y++)
        {
            for (int x = -ChunkRadius; x <= ChunkRadius; x++)
            {
                tilemap.SetTile(new Vector3Int((ChunkSize * chunk.Coords.x) + x, (ChunkSize * chunk.Coords.y) + y, 0), null);
            }
        }
        if (chunk.PlanetObjects == null) return;

        foreach (var obj in chunk.PlanetObjects)
        {
            if (obj != null)
            {
                var networkComp = obj.GetComponent<NetworkObject>();
                if (networkComp != null)
                {
                    networkComp.Despawn();
                    continue;
                }
                Destroy(obj);
            }
        }

        chunk.PlanetObjects = null;
    }

    protected override PlanetChunk GenerateChunk(Vector2Int coords)
    {
        PlanetChunk chunk = new()
        {
            Coords = coords,
            PlanetObjects = new()
        };

        for (int y = -ChunkRadius; y <= ChunkRadius; y++)
        {
            for (int x = -ChunkRadius; x <= ChunkRadius; x++)
            {
                float xCoord = ((ChunkSize * coords.x) + x) / (ChunkSize * scale);
                float yCoord = ((ChunkSize * coords.y) + y) / (ChunkSize * scale);
                double noiseValue = NoiseS3D.Noise(xCoord, yCoord);

                PlanetPaletteLayer layer = new();

                foreach (var l in planetPaletteBag.Palette)
                {
                    if (l.level >= noiseValue)
                    {
                        layer = l;
                        break;
                    }
                }

                int i = (int)(Math.Abs(noiseValue) * 100) % layer.tiles.Length;
                var tile = layer.tiles[i];

                tilemap.SetTile(new Vector3Int((ChunkSize * coords.x) + x, (ChunkSize * coords.y) + y, 0), tile);

                if (/*tile.colliderType == Tile.ColliderType.Sprite && */UnityEngine.Random.Range(0, 300) == 123 && IsServer)
                {
                    var enemy = Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], new Vector3Int((ChunkSize * coords.x) + x, (ChunkSize * coords.y) + y, -1), Quaternion.identity);
                    enemy.GetComponent<NetworkObject>().Spawn();
                    chunk.PlanetObjects.Add(enemy);
                }
                else if (UnityEngine.Random.Range(0, 300) == 123 && IsServer)
                {
                    var tree = Instantiate(treePrefabs[UnityEngine.Random.Range(0, treePrefabs.Length)], new Vector3Int((ChunkSize * coords.x) + x, (ChunkSize * coords.y) + y, -1), Quaternion.identity);
                    tree.GetComponent<NetworkObject>().Spawn();
                    chunk.PlanetObjects.Add(tree);
                }
                else if (UnityEngine.Random.Range(0, 300) == 123 && IsServer)
                {
                    var bush = Instantiate(bushPrefabs[UnityEngine.Random.Range(0, bushPrefabs.Length)], new Vector3Int((ChunkSize * coords.x) + x, (ChunkSize * coords.y) + y, -1), Quaternion.identity);
                    bush.GetComponent<NetworkObject>().Spawn();
                    chunk.PlanetObjects.Add(bush);
                }
                else if (UnityEngine.Random.Range(0, 300) == 123 && IsServer)
                {
                    var stone = Instantiate(stonePrefabs[UnityEngine.Random.Range(0, stonePrefabs.Length)], new Vector3Int((ChunkSize * coords.x) + x, (ChunkSize * coords.y) + y, -1), Quaternion.identity);
                    stone.GetComponent<NetworkObject>().Spawn();
                    chunk.PlanetObjects.Add(stone);
                }
                else if (UnityEngine.Random.Range(0, 300) == 123 && IsServer)
                {
                    var crystal = Instantiate(crystalPrefabs[UnityEngine.Random.Range(0, crystalPrefabs.Length)], new Vector3Int((ChunkSize * coords.x) + x, (ChunkSize * coords.y) + y, -1), Quaternion.identity);
                    crystal.GetComponent<NetworkObject>().Spawn();
                    chunk.PlanetObjects.Add(crystal);
                }
            }
        }

        return chunk;
    }
}