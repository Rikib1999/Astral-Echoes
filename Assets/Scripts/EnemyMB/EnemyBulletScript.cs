using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    


    public float force = 5;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {

 
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        
        Vector3 direction = player.transform.position - transform.position;

        rb.velocity = new Vector2(direction.x, direction.y-1).normalized * force;

        float rot = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rot+90);



    }

    // Update is called once per frame
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
        Debug.Log("Hit");
    
    }


    void GetDamaged(GameObject player)
    {
        PlayerLogic playerLogic = player.GetComponent<PlayerLogic>();

        if(playerLogic != null)
        {
            playerLogic.damage(2);
        }
        else
        {
            Debug.Log("PlayerLogic is null");
        }

        //Debug.Log("HIT");
        //Debug.Log("Player Health: " + player.GetComponent<PlayerLogic>().health);
    }
}
