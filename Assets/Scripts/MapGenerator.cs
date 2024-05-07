using UnityEngine;

public class MapGenerator : ChunkGenerator
{
	private const double objectTreshold = 0.95;

	private new void Start()
	{
		base.Start();
	}

	protected override void DeleteChunk(Vector2Int coords)
	{
		throw new System.NotImplementedException();
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

				if (noiseValue > objectTreshold)
				{
					//new 
				}
			}
		}
	}
}