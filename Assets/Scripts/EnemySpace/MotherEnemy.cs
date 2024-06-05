using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherEnemy : MonoBehaviour
{
    public GameObject player;
    public float speed;
    //public float size;
    public float StopToSpawnDistance = 10f; // Distance at which the enemy starts chasing the player
    public float roamRadius = 3f; // Radius around the roamPosition where the enemy can roam
    public float roamSpeed = 2f; // Speed at which the enemy roams
    public bool chase;
    public float publicDistance;
    public float timer;

    private float distance;
    private Vector2 roamPosition;
    private Vector2 roamTarget;
    private SpawnShooting spawn;
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        chase = false;
        timer = 6;
        spawn = GetComponent<SpawnShooting>();

        //size = Random.Range(0.5f, 2f);
        //transform.localScale = new Vector3(size, size, 1);

        // Set initial roam position to the enemy's starting position
        roamPosition = transform.position;
        SetNewRoamTarget();
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        publicDistance = distance;

        timer -= Time.deltaTime;
        
        //Debug.Log("Distance: "+publicDistance);

        if (distance < StopToSpawnDistance)
        {
            // Chase the player
            //chase = true;
            //ChasePlayer();
            //Debug.Log("SpawnIT NOW");
            if(timer <= 0)
            {
                spawn.SpawnShip();
                timer = 6;
            }
            //spawn.SpawnShip();
            
        }
        else
        {
            // Roam around the roamPosition
            //chase = false;
            Roam();
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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
