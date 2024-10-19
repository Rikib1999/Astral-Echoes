using UnityEngine.UI;
using UnityEngine;

namespace Assets.Scripts.SpaceshipInterior
{
    public class BackgroundScroller : MonoBehaviour
    {
        private RawImage rawImage;

        [SerializeField] private float multiplier;

        void Start()
        {
            rawImage = GetComponent<RawImage>();
        }

        void Update()
        {
            rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(rawImage.uvRect.position.x, multiplier) * Time.deltaTime, rawImage.uvRect.size);
        }
    }
}