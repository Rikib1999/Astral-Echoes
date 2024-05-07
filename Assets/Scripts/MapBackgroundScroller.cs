using UnityEngine;
using UnityEngine.UI;

public class MapBackgroundScroller : MonoBehaviour
{
    private RawImage rawImage;

    [SerializeField] private float sensitivity = 0.005f;
    [SerializeField] private Camera cam;

    void Start()
    {
        rawImage= GetComponent<RawImage>();
    }

    void Update()
    {
        rawImage.uvRect = new Rect(cam.transform.position * sensitivity, rawImage.uvRect.size);
    }
}