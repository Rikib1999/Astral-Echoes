using UnityEngine;

public class EnemyBoom : MonoBehaviour
{
    private BombEnemy bombE;
    private GameObject player;

    public float timer;

    private void Start()
    {
        bombE = GetComponent<BombEnemy>();
        player = GameObject.FindGameObjectWithTag("Player");
        timer = 5;
    }

    private void Update()
    {
        if (bombE.chase == true)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                GetComponent<ParticleSystem>().Play();

                if (bombE.publicDistance < 40)
                {
                    player.GetComponent<ShipHealth>().damage(50);
                }
            }
        }
        else
        {
            timer = 10;
        }
    }
}