using UnityEngine;

public class PatternShooter : MonoBehaviour
{
    public GameObject bulletPrefab;   // Bullet prefab
    public float shootInterval = 2f; // Time between shots
    public float bulletSpeed = 5f;   // Speed of the bullets
    public int numberOfBullets = 8;  // Number of bullets to shoot
    public PatternType patternType;  // Shooting pattern

    private float shootTimer;

    public enum PatternType
    {
        Circle,
        Pentagon,
        Star,
        Custom // Allows custom-defined points
    }

    private void Update()
    {
        // Timer for shooting
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval; // Reset timer
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
                ShootCustom(); // Define your custom pattern logic
                break;
        }
    }

    private void ShootInCircle()
    {
        float angleStep = 360f / numberOfBullets; // Evenly spaced angles
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
        int points = numberOfBullets / 2; // Number of points in the star
        float angleStep = 360f / points;
        for (int i = 0; i < points; i++)
        {
            float innerAngle = i * angleStep;             // Inner point angle
            float outerAngle = innerAngle + angleStep / 2; // Outer point angle
            SpawnBulletAtAngle(innerAngle);
            SpawnBulletAtAngle(outerAngle);
        }
    }

    private void ShootCustom()
    {
        // Example: Custom points
        Vector2[] customOffsets = { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
        foreach (Vector2 offset in customOffsets)
        {
            SpawnBullet(transform.position + (Vector3)offset, offset.normalized * bulletSpeed);
        }
    }

    private void SpawnBulletAtAngle(float angle)
    {
        // Convert angle to radians and calculate direction
        float radians = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        SpawnBullet(transform.position, direction * bulletSpeed);
    }

    private void SpawnBullet(Vector3 position, Vector2 velocity)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = velocity;
        }
    }
}
