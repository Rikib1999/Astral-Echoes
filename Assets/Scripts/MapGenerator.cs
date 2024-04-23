using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public float scale = 0.5F;

    private Tilemap tilemap;

    //for now hardcoded, later in static MapManager based on current planet and seed
    private Dictionary<double, Tile> palette;

    public Tile tileWater;
    public Tile tileSand;
    public Tile tileGrass;

    private const int chunkSize = 32;
    private const int maxExistingChunks = 64;
    private const int visibleCoordsDistance = 3;

    private int ChunkRadius => chunkSize / 2;

    private List<TileChunk> chunkList;
    private Dictionary<Vector2Int, TileChunk> chunksByCoords;

    private Vector2Int playerCoords;
    private bool playerCoordsChanged;

    public Transform player;

    private void Start()
    {
        //TODO: remove
        palette = new Dictionary<double, Tile>()
        {
            { 0.3, tileWater },
            { 0.5, tileSand },
            { int.MaxValue, tileGrass },
        };

        tilemap = GetComponent<Tilemap>();
        chunkList = new();
        chunksByCoords = new();
        playerCoordsChanged = true;
    }

    private void Update()
    {
        if (playerCoordsChanged)
        {
            UpdateMap();
        }

        UpdatePLayerCoords();
    }

    private void UpdatePLayerCoords()
    {
        playerCoordsChanged = false;

        Vector2Int tmp = playerCoords;
        playerCoords = new Vector2Int(Mathf.FloorToInt(player.position.x) / chunkSize, Mathf.FloorToInt(player.position.y) / chunkSize);

        if (playerCoords.x != tmp.x || playerCoords.y != tmp.y) playerCoordsChanged = true;
    }

    private void UpdateMap()
    {
        SortChunksByDistance();
        DeleteChunks();
        GenerateChunks();
        SetVisibility();
    }

    private void SortChunksByDistance()
    {
        foreach (var chunk in chunkList)
        {
            chunk.coordsDistance = Vector2.Distance(playerCoords, chunk.coords);
        }

        chunkList = chunkList.OrderBy(x => x.coordsDistance).ToList();
    }

    private void DeleteChunks()
    {
        while (chunkList.Count > maxExistingChunks)
        {
            TileChunk chunk = chunkList[chunkList.Count - 1];
            chunkList.Remove(chunk);
            chunksByCoords.Remove(chunk.coords);
            DestroyTiles(chunk.coords);
        }
    }

    private void DestroyTiles(Vector2Int coords)
    {
        for (int y = -ChunkRadius; y <= ChunkRadius; y++)
        {
            for (int x = -ChunkRadius; x <= ChunkRadius; x++)
            {
                tilemap.SetTile(new Vector3Int((chunkSize * coords.x) + x, (chunkSize * coords.y) + y, 0), null);
            }
        }
    }

    //TODO: setActive
    private void SetVisibility()
    {
        foreach (var chunk in chunkList)
        {
            if (chunk.coordsDistance > visibleCoordsDistance) chunk.visible = false;
            else chunk.visible = true;
        }
    }

    private void GenerateChunks()
    {
        for (int xOffset = -visibleCoordsDistance; xOffset <= visibleCoordsDistance; xOffset++)
        {
            for (int yOffset = -visibleCoordsDistance; yOffset <= visibleCoordsDistance; yOffset++)
            {
                Vector2Int coords = new(playerCoords.x + xOffset, playerCoords.y + yOffset);

                if (!chunksByCoords.ContainsKey(coords))
                {
                    TileChunk chunk = new() { coords = coords };

                    chunksByCoords.Add(coords, chunk);
                    chunkList.Add(chunk);

                    GenerateTiles(coords);
                }
            }
        }
    }

    /*
    private double S3DPerlin(float x, float y)
    {
        double sample = NoiseS3D.Noise(x, y);
        return Mathf.PerlinNoise((float)sample, (float)sample);
    }
    */

    private void GenerateTiles(Vector2Int coords)
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