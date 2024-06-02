using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject crosshair;

    // Start is called before the first frame update
    void Start()
    {

        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        crosshair.transform.position = mousePos;

        Vector3 dir = crosshair.transform.position - transform.position;
        

        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        transform.up = direction;


        if (Input.GetKey(KeyCode.W)){

            //follow where the crosshair is pointing
            transform.position = Vector2.MoveTowards(transform.position, crosshair.transform.position, 5 * Time.deltaTime);


        }
        

        
    }
}
