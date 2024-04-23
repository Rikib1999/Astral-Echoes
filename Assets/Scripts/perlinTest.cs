using UnityEngine;
using UnityEngine.Tilemaps;

public class perlinTest : MonoBehaviour
{
    public enum eNoise
    {
        Perlin = 1,
        S3D = 2,
        S3DPerlin = 3,
        S3DCombined = 4
    }

    public float xOrg;
    public float yOrg;

    public eNoise noiseType = eNoise.S3D;
    public float scale = 1.0F;
    public int octaves = 1;

    public TileBase tileGrass;
    public TileBase tileDirt;

    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    void CalcNoise()
    {
        int height = 50;
        int width = 50;

        for (int y = -height; y <= height; y++)
        {
            for (int x = -width; x <= width; x++)
            {
                float xCoord = xOrg + x / (width * scale);
                float yCoord = yOrg + y / (height * scale);

                NoiseS3D.octaves = octaves;
                double sample = 0;

                switch (noiseType)
                {
                    case eNoise.Perlin:
                        sample = Mathf.PerlinNoise(xCoord, yCoord);
                        break;
                    case eNoise.S3D:
                        sample = NoiseS3D.Noise(xCoord, yCoord);
                        break;
                    case eNoise.S3DPerlin:
                        sample = S3DPerlin(xCoord, yCoord);
                        break;
                    case eNoise.S3DCombined:
                        sample = NoiseS3D.NoiseCombinedOctaves(xCoord, yCoord);
                        break;
                    default:
                        sample = NoiseS3D.Noise(xCoord, yCoord);
                        break;
                }

                if (sample > 0.5)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileGrass);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileDirt);
                }
            }
        }
    }

    private double S3DPerlin(float x, float y)
    {
        double sample = NoiseS3D.Noise(x, y);
        return Mathf.PerlinNoise((float)sample, (float)sample);
    }

    void Update()
    {
        CalcNoise();
    }
}