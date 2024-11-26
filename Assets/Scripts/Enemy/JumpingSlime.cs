using UnityEngine;

public class JumpingSlime : MonoBehaviour
{
    public Transform player;            // Reference to the player
    public float jumpForce = 300f;      // Force applied to the slime's jump
    public float jumpInterval = 2f;     // Time between jumps

    private Rigidbody2D rb;             // Rigidbody2D component
    private float jumpTimer = 0f;       // Timer for managing jumps

    private void Start()
    {
        // Cache the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Find the player if not assigned
        if (player == null && GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (player == null) return; // Exit if no player is assigned

        // Manage jump timing
        jumpTimer += Time.deltaTime;
        if (jumpTimer >= jumpInterval)
        {
            JumpTowardsPlayer();
            jumpTimer = 0f; // Reset the timer
        }
    }

    private void JumpTowardsPlayer()
    {
        // Calculate the direction toward the player
        Vector2 jumpDirection = (player.position - transform.position).normalized;

        // Apply a force in the direction of the player
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
    }
}
