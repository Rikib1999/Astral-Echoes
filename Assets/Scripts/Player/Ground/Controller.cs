using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using Assets.Scripts.Player;

public class Controller : NetworkBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform[] guns; // Array to store references to all guns
    [SerializeField] GameObject crosshair;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] int maxHealth = 100; // Max player health
    [SerializeField] Slider healthBar; // Health UI element
    [SerializeField] Slider dashBar; // Dash UI element

    private float dirX = 0;
    private float dirY = 0;
    private int currentHealth;
    private bool facingRight = true;
    private bool isDashing = false;
    private float lastDashTime = -10f;
    private Animator animator;
    private int activeGunIndex = 0; // Index of the currently active gun
    private AudioSource audioSource;
    [SerializeField] AudioSource dashAudioSource;
    private SpriteRenderer spriteRenderer;
    private CameraFollow playerCamera;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) // Disable this script if not owner
        {
            enabled = false;
            crosshair.SetActive(false);
        }
        else // Set the camera to follow
        {
            playerCamera = FindObjectOfType<CameraFollow>();
            playerCamera.Target = this.transform;
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        // Initialize UI values
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        dashBar.maxValue = dashCooldown;
        dashBar.value = dashCooldown;

        // Deactivate all guns except the first one
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].gameObject.SetActive(i == activeGunIndex);
        }
    }

    private void Update()
    {
        // Move the crosshair with the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        crosshair.transform.position = mousePosition;

        // Determine if the mouse has moved left or right
        facingRight = crosshair.transform.position.x > transform.position.x;

        // Flip the player sprite accordingly
        if (facingRight && transform.localScale.x < 0 || !facingRight && transform.localScale.x > 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        // Update movement input
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxis("Vertical");

        // Update walking animation and sound
        bool isWalking = dirX * moveSpeed != 0 || dirY * moveSpeed != 0;
        animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else if (audioSource.isPlaying) audioSource.Stop();

        // Update dash cooldown UI
        dashBar.value = Mathf.Clamp(Time.time - lastDashTime, 0, dashCooldown);

        // Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }

        // Switch active gun based on player input
        if (Input.GetKeyDown(KeyCode.Alpha1) && guns.Length >= 1)
        {
            SwitchGun(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && guns.Length >= 2)
        {
            SwitchGun(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && guns.Length >= 3)
        {
            SwitchGun(2);
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            // Move the player based on input
            Vector2 movement = new Vector2(dirX, dirY).normalized * moveSpeed;
            rb.velocity = movement;
        }
    }

    private void LateUpdate()
    {
        RotateWeapon();
    }

    private void RotateWeapon()
    {
        Vector3 targetPosition = crosshair.transform.position;
        Vector3 direction = targetPosition - guns[activeGunIndex].position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (!facingRight) angle += 180f;
        guns[activeGunIndex].rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void SwitchGun(int newIndex)
    {
        guns[activeGunIndex].gameObject.SetActive(false);
        guns[newIndex].gameObject.SetActive(true);
        activeGunIndex = newIndex;
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        dashAudioSource.Play();

        // Set alpha to 50% (semi-transparent) at the start of the dash
        ChangeAlpha(0.5f);

        Vector2 dashDirection = new Vector2(dirX, dirY).normalized;
        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        // Reset alpha to fully visible after the dash
        ChangeAlpha(1f);

        isDashing = false;
    }

    // Method to change the player's sprite alpha
    private void ChangeAlpha(float alphaValue)
    {
        Color color = spriteRenderer.color;
        color.a = alphaValue; // Change only the alpha, keeping the rest of the color
        spriteRenderer.color = color;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            // Handle player death
        }
    }
}
