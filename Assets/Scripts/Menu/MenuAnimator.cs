using UnityEngine;
//Space Rock by SergeQuadrado -- https://freesound.org/s/671333/ -- License: Attribution NonCommercial 4.0
public class MenuAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer menuImage;  // Reference to the SpriteRenderer of the menu image
    [SerializeField] private float colorChangeSpeed = 0.5f; // Speed of the color change
    [SerializeField] private float changeAmount = 0.01f; // Amount by which RGB changes each frame

    private Color currentColor;

    void Start()
    {
        if (menuImage == null)
        {
            menuImage = GetComponent<SpriteRenderer>();
        }

        // Start with the current color of the sprite
        currentColor = menuImage.color;
    }

    void Update()
    {
        // Randomly change each color channel a bit (R, G, B)
        currentColor.r = Mathf.Clamp01(currentColor.r + Random.Range(-changeAmount, changeAmount) * colorChangeSpeed);
        currentColor.g = Mathf.Clamp01(currentColor.g + Random.Range(-changeAmount, changeAmount) * colorChangeSpeed);
        currentColor.b = Mathf.Clamp01(currentColor.b + Random.Range(-changeAmount, changeAmount) * colorChangeSpeed);

        // Assign the new color to the sprite
        menuImage.color = currentColor;
    }
}
