using Unity.Netcode;
using UnityEngine;

public class ShipController : NetworkBehaviour
{
    public GameObject crosshair;
    public float speed = 4f;

    // Called on client join
    public override void OnNetworkSpawn()
    {
        if(!IsOwner) //Disable this script if not owner
        {
            enabled=false;
            crosshair.SetActive(false);
        }
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        mousePos.z = 0;
        crosshair.transform.position = mousePos;
       
        Vector2 direction = new(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        transform.up = direction;

        FlyForward();
    }

    private void FlyForward()
    {

        if (Input.GetKey(KeyCode.W))
        {
            //follow where the crosshair is pointing
            transform.position = Vector2.MoveTowards(transform.position, crosshair.transform.position, speed * Time.deltaTime);
        }
    }
}