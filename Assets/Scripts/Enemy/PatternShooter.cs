using UnityEngine;

public class PatternShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval = 2f;
    public float bulletSpeed = 5f;
    public int numberOfBullets = 8;
    public PatternType patternType;

    private float shootTimer;

    public enum PatternType
    {
        Circle,
        Pentagon,
        Star,
        Custom
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    private void Shoot()
    {
        switch (patternType)
        {
            case PatternType.Circle:
                ShootInCircle();
                break;
            case PatternType.Pentagon:
                ShootInPolygon(5);
                break;
            case PatternType.Star:
                ShootInStar();
                break;
            case PatternType.Custom:
                ShootCustom();
                break;
        }
    }

    private void ShootInCircle()
    {
        float angleStep = 360f / numberOfBullets;
        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = i * angleStep;
            SpawnBulletAtAngle(angle);
        }
    }

    private void ShootInPolygon(int sides)
    {
        float angleStep = 360f / sides;
        for (int i = 0; i < sides; i++)
        {
            float angle = i * angleStep;
            SpawnBulletAtAngle(angle);
        }
    }

    private void ShootInStar()
    {
        int points = numberOfBullets / 2;
        float angleStep = 360f / points;
        for (int i = 0; i < points; i++)
        {
            float innerAngle = i * angleStep;
            float outerAngle = innerAngle + angleStep / 2;
            SpawnBulletAtAngle(innerAngle);
            SpawnBulletAtAngle(outerAngle);
        }
    }

    private void ShootCustom()
    {
        Vector2[] customOffsets = { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
        foreach (Vector2 offset in customOffsets)
        {
            SpawnBullet(transform.position + (Vector3)offset, offset.normalized * bulletSpeed);
        }
    }

    private void SpawnBulletAtAngle(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        Vector2 direction = new(Mathf.Cos(radians), Mathf.Sin(radians));

        SpawnBullet(transform.position, direction * bulletSpeed);
    }

    private void SpawnBullet(Vector3 position, Vector2 velocity)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        if (bullet.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.velocity = velocity;
        }
    }
}