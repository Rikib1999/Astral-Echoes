using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemies : MonoBehaviour
{
    public GameObject player;

    public float speed;

    public float size;

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        size = Random.Range(0.5f, 2f);

        transform.localScale = new Vector3(size, size, 1);

        
    }

    // Update is called once per frame
    void Update()
    {
        
        distance = Vector2.Distance(transform.position, player.transform.position);

        Vector2 direction = player.transform.position - transform.position;

        direction.Normalize();

        float angle = Mathf.Atan2(direction.y-0.25f, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));



    }
}
