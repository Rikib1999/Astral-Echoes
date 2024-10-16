using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
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
    [SerializeField] float dirX = 0;
    [SerializeField] float dirY = 0;
    bool facingRight = true;
    bool isDashing = false;
    float dashTime = 0;
    float lastDashTime = -10f;
    Animator animator;
    private Vector3 lastMousePosition;
    private int activeGunIndex = 0; // Index of the currently active gun
    AudioSource audioSource;
    [SerializeField] AudioSource dashAudioSource;
    private CameraFollow playerCamera;
    SpriteRenderer spriteRenderer;

    // Called on client join
    public override void OnNetworkSpawn()
    {
        if(!IsOwner) //Disable this script if not owner
        {
            //Destroy(this);
            enabled=false;
            crosshair.SetActive(false);
        }
        else //Set the camera to follow
        {
            playerCamera = FindObjectOfType<CameraFollow>();
            playerCamera.Target = this.transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
        audioSource = GetComponent<AudioSource>();  
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        lastMousePosition = Input.mousePosition;

        // Deactivate all guns except the first one
        for (int i = 0; i < guns.Length; i++)
        {
            if (i != activeGunIndex)
            {
                guns[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // Move the crosshair with the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        crosshair.transform.position = mousePosition;

        // Determine if the mouse has moved left or right
        if (crosshair.transform.position.x > transform.position.x)
        {
            facingRight = true;
        }
        else if (crosshair.transform.position.x < transform.position.x)
        {
            facingRight = false;
        }

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

        if (dirX * moveSpeed != 0 || dirY * moveSpeed != 0)
        {
            animator.SetBool("isWalking", true);

            // Play walking sound if not already playing
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            animator.SetBool("isWalking", false);

            // Stop the sound when the player stops walking
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

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

        if (!facingRight)
        {
            angle += 180f;
        }
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
        dashTime = Time.time;
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
}
