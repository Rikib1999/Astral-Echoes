using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlanetGenerator : ChunkGenerator<Chunk>
{
    private float scale = 0.5f;

    protected override int ChunkSize { get; set; } = 32;
    protected override int MaxExistingChunks { get; set; } = 64;
    protected override int VisibleCoordsDistance { get; set; } = 3;

    [SerializeField] private Tilemap tilemap;

    //for now hardcoded, later in static MapManager based on current planet and seed
    private Dictionary<double, Tile> palette;

    public Tile tileWater;
    public Tile tileSand;
    public Tile tileGrass;

    private new void Start()
    {
        base.Start();

        //TODO: remove
        palette = new Dictionary<double, Tile>()
        {
            { 0.3, tileWater },
            { 0.5, tileSand },
            { int.MaxValue, tileGrass },
        };
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
                Tile tile = null;

                foreach (var layer in palette)
                {
                    if (layer.Key >= noiseValue)
                    {
                        tile = layer.Value;
                        break;
                    }
                }

                tilemap.SetTile(new Vector3Int((ChunkSize * coords.x) + x, (ChunkSize * coords.y) + y, 0), tile);
            }
        }

        return new() { Coords = coords };
    }
}