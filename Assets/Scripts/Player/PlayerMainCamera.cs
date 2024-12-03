using UnityEngine;
using Unity.Netcode;

public class PlayerMainCamera : NetworkBehaviour
{
    public GameObject camera;

    void Start()
    {
        if(IsOwner)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    void Update()
    {
        if (camera)
        {
            camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
        }
    }
}