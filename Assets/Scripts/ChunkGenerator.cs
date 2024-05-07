using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ChunkGenerator : MonoBehaviour
{
    public float scale = 0.5F;

    protected const int chunkSize = 32;
    protected const int maxExistingChunks = 64;
    protected const int visibleCoordsDistance = 3;

    protected int ChunkRadius => chunkSize / 2;

    protected List<Chunk> chunkList;
    protected Dictionary<Vector2Int, Chunk> chunksByCoords;

    protected Vector2Int playerCoords;
    protected bool playerCoordsChanged;

    public Transform player;

    protected void Start()
    {
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

        UpdatePlayerCoords();
    }

    private void UpdatePlayerCoords()
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
            Chunk chunk = chunkList[chunkList.Count - 1];
            chunkList.Remove(chunk);
            chunksByCoords.Remove(chunk.coords);
            DeleteChunk(chunk.coords);
        }
    }

    protected abstract void DeleteChunk(Vector2Int coords);

    private void GenerateChunks()
    {
        for (int xOffset = -visibleCoordsDistance; xOffset <= visibleCoordsDistance; xOffset++)
        {
            for (int yOffset = -visibleCoordsDistance; yOffset <= visibleCoordsDistance; yOffset++)
            {
                Vector2Int coords = new(playerCoords.x + xOffset, playerCoords.y + yOffset);

                if (!chunksByCoords.ContainsKey(coords))
                {
                    Chunk chunk = new() { coords = coords };

                    chunksByCoords.Add(coords, chunk);
                    chunkList.Add(chunk);

                    GenerateChunk(coords);
                }
            }
        }
    }

    protected abstract void GenerateChunk(Vector2Int coords);

    private void SetVisibility()
    {
        foreach (var chunk in chunkList)
        {
            if (chunk.coordsDistance > visibleCoordsDistance) chunk.visible = false;
            else chunk.visible = true;
        }
    }

    /*
    private double S3DPerlin(float x, float y)
    {
        double sample = NoiseS3D.Noise(x, y);
        return Mathf.PerlinNoise((float)sample, (float)sample);
    }
    */
}