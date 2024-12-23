using System.Linq;
using UnityEngine;

public class EnemyBulletScriptPlanet : MonoBehaviour
{
    private GameObject targetPlayer;
    private Rigidbody2D rb;
    public float force = 5;
    public float timer;

    void Start()
    {
        FindClosestPlayer();
        rb = GetComponent<Rigidbody2D>();

        Vector3 direction = targetPlayer.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot+90);
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
            .FirstOrDefault()?.gameObject;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 10)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetDamaged(other.gameObject);
            Destroy(gameObject);
        }
    }

    void GetDamaged(GameObject player)
    {
        PlayerLogic playerLogic = player.GetComponent<PlayerLogic>();        

        if(playerLogic != null)
        {
            playerLogic.damage(1);
        }
    }
}