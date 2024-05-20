using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    private const float minCamSize = 20f;
    public const float maxCamSize = 400f;

    private Camera cam;
    private Vector3 dragOrigin;
    
    [SerializeField] private float zoomSpeed = 40f;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            cam.transform.position += difference;
        }

        if (Input.GetAxis("Mouse ScrollWheel") == 0) return;

        cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minCamSize, maxCamSize);
    }
}