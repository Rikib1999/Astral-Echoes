using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyBoom : MonoBehaviour
{
    private BombEnemy bombE;
    private GameObject player;

    public float timer;

    // Start is called before the first frame update
    void Start()
    {

        bombE = GetComponent<BombEnemy>();
        player = GameObject.FindGameObjectWithTag("Player");
        timer = 10;
        
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;



        if (bombE.chase == true)
        {
            
            
            timer -= Time.deltaTime;

            Debug.Log(timer);

            if (timer <= 0)
            {
                Debug.Log("Boom");
                Destroy(gameObject);

                if(bombE.publicDistance < 3)
                {
                    player.GetComponent<ShipHealth>().damage(50);

                    Debug.Log("Player Hit");
                }

            }

        }
        else
        {
            timer=10;
            //Debug.Log("No Boom");
        }



    }
}
