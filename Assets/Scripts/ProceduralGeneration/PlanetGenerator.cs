using Assets.Scripts;
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
    [SerializeField] private PlanetPalette planetPalette;

    private PlanetPaletteBag planetPaletteBag;

    private new void Start()
    {
        base.Start();

        planetPaletteBag = new(planetPalette);
        NoiseS3D.seed = PlanetMapManager.Seed;
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