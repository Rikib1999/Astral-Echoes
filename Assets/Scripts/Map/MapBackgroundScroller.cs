using UnityEngine;
using UnityEngine.UI;

public class MapBackgroundScroller : MonoBehaviour
{
    private RawImage rawImage;
    private float multiplier;

    [SerializeField] private float sensitivity = 0.00000025f;
    [SerializeField] private Camera cam;


    void Start()
    {
        rawImage = GetComponent<RawImage>();
        multiplier = CameraScrolling.maxCamSize * sensitivity;
    }

    void Update()
    {
        rawImage.uvRect = new Rect(cam.transform.position * multiplier, rawImage.uvRect.size);
    }
}