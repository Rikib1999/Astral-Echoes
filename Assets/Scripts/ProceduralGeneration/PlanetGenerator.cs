using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.Planet;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlanetGenerator : ChunkGenerator<PlanetChunk>
{
    private readonly float scale = 0.5f;

    protected override int ChunkSize { get; set; } = 32;
    protected override int MaxExistingChunks { get; set; } = 32;
    protected override int VisibleCoordsDistance { get; set; } = 2;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private PlanetPalettesList planetPalettesList;
    [SerializeField] private GameObject[] enemyPrefabs;

    private PlanetPaletteBag planetPaletteBag;

    private new void Start()
    {
        base.Start();

        planetPaletteBag = new(GetPlanetPalette());
        NoiseS3D.seed = PlanetMapManager.Seed;
    }

    private PlanetPalette GetPlanetPalette()
    {
        switch ((ePlanetType)PlanetMapManager.PlanetDataBag.SubType)
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

    protected override void DeleteChunk(PlanetChunk chunk)
    {
        for (int y = -ChunkRadius; y <= ChunkRadius; y++)
        {
            for (int x = -ChunkRadius; x <= ChunkRadius; x++)
            {
                tilemap.SetTile(new Vector3Int((ChunkSize * chunk.Coords.x) + x, (ChunkSize * chunk.Coords.y) + y, 0), null);
            }
        }

        if (chunk.Enemies == null) return;

        foreach (var obj in chunk.Enemies) Destroy(obj);

        chunk.Enemies = null;
    }

    protected override PlanetChunk GenerateChunk(Vector2Int coords)
    {
        PlanetChunk chunk = new()
        {
            Coords = coords,
            Enemies = new()
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

                if (tile.colliderType == Tile.ColliderType.None)
                {
                    if (UnityEngine.Random.Range(0, 100) % 7 == 0)
                    {
                        Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], new Vector3Int((ChunkSize * coords.x) + x, (ChunkSize * coords.y) + y, -1), Quaternion.identity);
                    }
                }
            }
        }

        return chunk;
    }
}