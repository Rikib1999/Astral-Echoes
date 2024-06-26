using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletControl : MonoBehaviour
{
    //private GameObject player;
    private Rigidbody2D rb;
    public GameObject crosshair;


    public float force = 8;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        //player = GameObject.FindGameObjectWithTag("Player");
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");

        Vector3 direction = crosshair.transform.position - transform.position;

        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rot + 90);

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
        if (other.gameObject.CompareTag("Enemy"))
        {
            GetDamage(other.gameObject);
            Destroy(gameObject);
        }
        Debug.Log("Hit enemy");

    }
    
    void GetDamage(GameObject enemy)
    {
        EnemyShipHealth eshipHealth = enemy.GetComponent<EnemyShipHealth>();
    
        if(eshipHealth != null)
        {
            eshipHealth.damage(5);
        }
        else
        {
            Debug.Log("EnemyShipHealth is null");
        }
    
    }



}
