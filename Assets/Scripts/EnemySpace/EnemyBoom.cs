using System.Linq;
using UnityEngine;

public class EnemyBoom : MonoBehaviour
{
    private BombEnemy bombE;
    private GameObject targetPlayer;

    public float timer;

    private void Start()
    {
        bombE = GetComponent<BombEnemy>();
        timer = 5;
    }

    private void Update()
    {
        FindClosestPlayer();

        //chase player
        if (bombE.chase == true)
        {
            timer -= Time.deltaTime;

            //if timer gets to zero explode and damage player
            if (timer <= 0)
            {
                GetComponent<ParticleSystem>().Play();

                if (bombE.publicDistance < 40)
                {
                    targetPlayer.GetComponent<ShipHealth>().damage(50);
                }
            }
        }
        else
        {
            timer = 10;
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
            .FirstOrDefault()?.gameObject;
    }
}