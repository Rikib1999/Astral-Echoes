using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;

// Abstract base class for generating chunks of type T (derived from Chunk)
// T must inherit from Chunk
public abstract class ChunkGenerator<T> : NetworkBehaviour where T : Chunk
{
    // Abstract properties to be defined in derived classes
    protected abstract int ChunkSize { get; set; } // Size of a chunk (e.g., in world units)
    protected abstract int MaxExistingChunks { get; set; } // Maximum number of chunks to keep loaded
    protected abstract int VisibleCoordsDistance { get; set; } // Radius of chunk visibility in chunk coordinates

    protected int ChunkRadius => ChunkSize / 2; // Radius of a chunk, derived from its size

    protected List<T> chunkList; // List of all currently loaded chunks
    protected Dictionary<Vector2Int, T> chunksByCoords; // Dictionary to map chunk coordinates to chunk objects

    protected Vector2Int playerCoords; // The chunk coordinates of the player
    protected bool playerCoordsChanged; // Tracks if the player's chunk coordinates have changed

    public Transform player; // Reference to the player's transform

    // Initialization method
    protected void Start()
    {
        chunkList = new List<T>(); // Initialize chunk list
        chunksByCoords = new Dictionary<Vector2Int, T>(); // Initialize dictionary for chunks by coordinates
        playerCoordsChanged = true; // Assume player's position is new on start
    }

    private void Update()
    {
        // Skip processing if the player reference is not assigned
        if (player == null) return;

        // Update the map if the player's chunk coordinates have changed
        if (playerCoordsChanged)
        {
            UpdateMap();
        }

        // Update player's chunk coordinates
        UpdatePlayerCoords();
    }

    private void UpdatePlayerCoords()
    {
        playerCoordsChanged = false; // Reset flag

        // Temporarily store current player coordinates
        Vector2Int tmp = playerCoords;

        // Calculate player's new chunk coordinates based on their world position
        playerCoords = new Vector2Int(
            Mathf.FloorToInt(player.position.x) / ChunkSize,
            Mathf.FloorToInt(player.position.y) / ChunkSize
        );

        // If the player's chunk has changed, set the flag to true
        if (playerCoords.x != tmp.x || playerCoords.y != tmp.y)
        {
            playerCoordsChanged = true;
        }
    }

    private void UpdateMap()
    {
        SortChunksByDistance(); // Sort chunks by distance to the player
        DeleteChunks(); // Remove chunks that exceed the max allowed
        GenerateChunks(); // Create new chunks within visible range
        SetVisibility(); // Update visibility of chunks
    }

    private void SortChunksByDistance()
    {
        // Calculate the distance of each chunk from the player's current chunk
        foreach (var chunk in chunkList)
        {
            chunk.CoordsDistance = Vector2.Distance(playerCoords, chunk.Coords);
        }

        // Sort chunks by distance in ascending order
        chunkList = chunkList.OrderBy(x => x.CoordsDistance).ToList();
    }

    private void DeleteChunks()
    {
        // Remove chunks from the list until only MaxExistingChunks remain
        while (chunkList.Count > MaxExistingChunks)
        {
            T chunk = chunkList[chunkList.Count - 1]; // Select the farthest chunk
            chunkList.Remove(chunk); // Remove it from the list
            chunksByCoords.Remove(chunk.Coords); // Remove it from the dictionary
            DeleteChunk(chunk); // Call the abstract method to handle chunk-specific cleanup
        }
    }

    // Abstract method to define how to delete a chunk in derived classes
    protected abstract void DeleteChunk(T chunk);

    private void GenerateChunks()
    {
        // Iterate through a square grid around the player's chunk coordinates
        for (int xOffset = -VisibleCoordsDistance; xOffset <= VisibleCoordsDistance; xOffset++)
        {
            for (int yOffset = -VisibleCoordsDistance; yOffset <= VisibleCoordsDistance; yOffset++)
            {
                Vector2Int coords = new(playerCoords.x + xOffset, playerCoords.y + yOffset);

                // Check if the chunk already exists in the dictionary
                if (!chunksByCoords.ContainsKey(coords))
                {
                    T chunk = GenerateChunk(coords); // Generate a new chunk

                    // Add the new chunk to the dictionary and list
                    chunksByCoords.Add(coords, chunk);
                    chunkList.Add(chunk);
                }
            }
        }
    }

    // Abstract method to define how to generate a chunk in derived classes
    protected abstract T GenerateChunk(Vector2Int coords);

    private void SetVisibility()
    {
        // Update the visibility of each chunk based on its distance from the player
        foreach (var chunk in chunkList)
        {
            if (chunk.CoordsDistance > VisibleCoordsDistance)
            {
                chunk.Visible = false; // Set chunk to invisible if outside visible range
            }
            else
            {
                chunk.Visible = true; // Set chunk to visible if within range
            }
        }
    }

    /*
    private double S3DPerlin(float x, float y)
    {
        double sample = NoiseS3D.Noise(x, y); // Sample 3D noise
        return Mathf.PerlinNoise((float)sample, (float)sample); // Apply Perlin noise to the result
    }
    */
}