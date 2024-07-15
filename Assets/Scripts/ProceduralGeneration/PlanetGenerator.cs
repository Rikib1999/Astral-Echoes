using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.Planet;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlanetGenerator : ChunkGenerator<Chunk>
{
    private readonly float scale = 0.5f;

    protected override int ChunkSize { get; set; } = 32;
    protected override int MaxExistingChunks { get; set; } = 32;
    protected override int VisibleCoordsDistance { get; set; } = 2;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private PlanetPalettesList planetPalettesList;

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

    protected override void DeleteChunk(Chunk chunk)
    {
        for (int y = -ChunkRadius; y <= ChunkRadius; y++)
        {
            for (int x = -ChunkRadius; x <= ChunkRadius; x++)
            {
                tilemap.SetTile(new Vector3Int((ChunkSize * chunk.Coords.x) + x, (ChunkSize * chunk.Coords.y) + y, 0), null);
            }
        }
    }

    protected override Chunk GenerateChunk(Vector2Int coords)
    {
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

                tilemap.SetTile(new Vector3Int((ChunkSize * coords.x) + x, (ChunkSize * coords.y) + y, 0), layer.tiles[i]);
            }
        }

        return new() { Coords = coords };
    }
}