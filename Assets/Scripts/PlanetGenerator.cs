using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlanetGenerator : ChunkGenerator
{
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

    protected override void DeleteChunk(Vector2Int coords)
    {
        for (int y = -ChunkRadius; y <= ChunkRadius; y++)
        {
            for (int x = -ChunkRadius; x <= ChunkRadius; x++)
            {
                tilemap.SetTile(new Vector3Int((chunkSize * coords.x) + x, (chunkSize * coords.y) + y, 0), null);
            }
        }
    }

    protected override void GenerateChunk(Vector2Int coords)
    {
        for (int y = -ChunkRadius; y <= ChunkRadius; y++)
        {
            for (int x = -ChunkRadius; x <= ChunkRadius; x++)
            {
                float xCoord = ((chunkSize * coords.x) + x) / (chunkSize * scale);
                float yCoord = ((chunkSize * coords.y) + y) / (chunkSize * scale);

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

                tilemap.SetTile(new Vector3Int((chunkSize * coords.x) + x, (chunkSize * coords.y) + y, 0), tile);
            }
        }
    }
}