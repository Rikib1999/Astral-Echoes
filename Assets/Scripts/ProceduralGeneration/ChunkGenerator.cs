using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ChunkGenerator<T> : Singleton<ChunkGenerator<T>> where T : Chunk
{
    protected abstract int ChunkSize { get; set; }
    protected abstract int MaxExistingChunks { get; set; }
    protected abstract int VisibleCoordsDistance { get; set; }

    protected int ChunkRadius => ChunkSize / 2;

    protected List<T> chunkList;
    protected Dictionary<Vector2Int, T> chunksByCoords;

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
        playerCoords = new Vector2Int(Mathf.FloorToInt(player.position.x) / ChunkSize, Mathf.FloorToInt(player.position.y) / ChunkSize);

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
            chunk.CoordsDistance = Vector2.Distance(playerCoords, chunk.Coords);
        }

        chunkList = chunkList.OrderBy(x => x.CoordsDistance).ToList();
    }

    private void DeleteChunks()
    {
        while (chunkList.Count > MaxExistingChunks)
        {
            T chunk = chunkList[chunkList.Count - 1];
            chunkList.Remove(chunk);
            chunksByCoords.Remove(chunk.Coords);
            DeleteChunk(chunk);
        }
    }

    protected abstract void DeleteChunk(T chunk);

    private void GenerateChunks()
    {
        for (int xOffset = -VisibleCoordsDistance; xOffset <= VisibleCoordsDistance; xOffset++)
        {
            for (int yOffset = -VisibleCoordsDistance; yOffset <= VisibleCoordsDistance; yOffset++)
            {
                Vector2Int coords = new(playerCoords.x + xOffset, playerCoords.y + yOffset);

                if (!chunksByCoords.ContainsKey(coords))
                {
                    T chunk = GenerateChunk(coords);

                    chunksByCoords.Add(coords, chunk);
                    chunkList.Add(chunk);
                }
            }
        }
    }

    protected abstract T GenerateChunk(Vector2Int coords);

    private void SetVisibility()
    {
        foreach (var chunk in chunkList)
        {
            if (chunk.CoordsDistance > VisibleCoordsDistance) chunk.Visible = false;
            else chunk.Visible = true;
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