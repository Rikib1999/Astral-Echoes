using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicEnemies : NetworkBehaviour
{
    public GameObject targetPlayer;
    public float speed;
    public float size;
    public float chaseDistance = 13f; // Distance at which the enemy starts chasing the player
    public float roamRadius = 3f; // Radius around the roamPosition where the enemy can roam
    public float roamSpeed = 2f; // Speed at which the enemy roams

    private float distance;
    private Vector2 roamPosition;
    private Vector2 roamTarget;

    void Start()
    {
        size = Random.Range(0.5f, 2f);
        transform.localScale = new Vector3(size, size, 1);

        // Set initial roam position to the enemy's starting position
        roamPosition = transform.position;
        SetNewRoamTarget();
    }

    void Update()
    {
        FindClosestPlayer();

        if (!IsServer){return;}

        // Destroy ship if scene is not SystemMap
        if (SceneManager.GetActiveScene().name != "SystemMap") Destroy(gameObject);

        if (targetPlayer)
        {
            distance = Vector2.Distance(transform.position, targetPlayer.transform.position);
        }

        if (targetPlayer && (distance < chaseDistance))
        {
            // Chase the player
            ChasePlayer();
        }
        else
        {
            // Roam around the roamPosition
            Roam();
        }
    }

    /// <summary>
    /// Finds the closest player within detection range.
    /// </summary>
    private void FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
        {
            targetPlayer = null;
            return;
        }

        targetPlayer = players
            .Select(player => player.transform)
            .OrderBy(player => Vector3.Distance(transform.position, player.position))
            .FirstOrDefault(player => Vector3.Distance(transform.position, player.position) <= chaseDistance)?.gameObject;
    }

    void ChasePlayer()
    {
        Vector2 direction = targetPlayer.transform.position - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(transform.position, targetPlayer.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    void Roam()
    {
        if (Vector2.Distance(transform.position, roamTarget) < 0.2f)
        {
            SetNewRoamTarget();
        }

        Vector2 direction = roamTarget - (Vector2)transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(transform.position, roamTarget, roamSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    void SetNewRoamTarget()
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomRadius = Random.Range(0f, roamRadius);
        roamTarget = roamPosition + new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * randomRadius;
    }
}
