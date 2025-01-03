using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombEnemy : MonoBehaviour
{
    public GameObject targetPlayer;
    public float speed;
    //public float size;
    public float chesDist = 6f; // Distance at which the enemy starts chasing the player
    public float roamRadius = 3f; // Radius around the roamPosition where the enemy can roam
    public float roamSpeed = 2f; // Speed at which the enemy roams
    public bool chase;
    public float publicDistance;

    private float distance;
    private Vector2 roamPosition;
    private Vector2 roamTarget;
    

    void Start()
    {
        chase = false;

        //size = Random.Range(0.5f, 2f);
        //transform.localScale = new Vector3(size, size, 1);

        // Set initial roam position to the enemy's starting position
        roamPosition = transform.position;
        SetNewRoamTarget();
    }

    void Update()
    {
        FindClosestPlayer();

        if (targetPlayer)
        {
            distance = Vector2.Distance(transform.position, targetPlayer.transform.position);
        }

        publicDistance = distance;

        //Debug.Log("Distance: "+publicDistance);

        if (targetPlayer && (distance < chesDist))
        {
            // Chase the player
            chase = true;
            ChasePlayer();
            
        }
        else
        {
            // Roam around the roamPosition
            chase = false;
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
            .FirstOrDefault(player => Vector3.Distance(transform.position, player.position) <= chesDist)?.gameObject;
    }

    /// <summary>
    /// Chase player if within the range
    /// </summary>
    void ChasePlayer()
    {
        Vector2 direction = targetPlayer.transform.position - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //move towards the player and rotate to him
        transform.position = Vector2.MoveTowards(transform.position, targetPlayer.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    /// <summary>
    /// Roam around the are, set as target
    /// </summary>
    void Roam()
    {
        //if player gets to the roamTarget, set new one
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

    /// <summary>
    /// Set area to roam around
    /// </summary>
    void SetNewRoamTarget()
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomRadius = Random.Range(0f, roamRadius);
        roamTarget = roamPosition + new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * randomRadius;
    }
}
