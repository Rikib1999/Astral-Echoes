using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject crosshair;
    public float speed = 4f;
    private Vector2 lastDirection;

    // Start is called before the first frame update
    void Start()
    {

        crosshair = GameObject.FindGameObjectWithTag("Crosshair");

 
        
    }

    // Update is called once per frame
    void Update()
    {
        ShipHealth shipHealth = GetComponent<ShipHealth>();
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        crosshair.transform.position = mousePos;

        Vector3 dir = crosshair.transform.position - transform.position;
        

        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        transform.up = direction;



        if(shipHealth.getFuel() > 0) FlyForward();
        else
        {
            Debug.Log("Out of fuel");
            DrifForward();
        }


        
        
    }

    private void FlyForward()
    {

        if (Input.GetKey(KeyCode.W))
        {
            // Calculate the direction towards the crosshair
            Vector2 direction = (crosshair.transform.position - transform.position).normalized;

            //follow where the crosshair is pointing
            transform.position = Vector2.MoveTowards(transform.position, crosshair.transform.position, speed * Time.deltaTime);

            lastDirection = direction;

        }

    }

    private void DrifForward()
    {
        float driftSpeed = speed * 0.1f;

        transform.position += (Vector3)lastDirection * driftSpeed * Time.deltaTime;
    }
}
