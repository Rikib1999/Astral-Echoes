using Assets.Scripts.Player;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Controller : NetworkBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform[] guns;
    [SerializeField] GameObject crosshair;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 2f;
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider dashBar;

    private float dirX = 0;
    private float dirY = 0;
    private bool facingRight = true;
    private bool isDashing = false;
    private float lastDashTime = -10f;
    private Animator animator;
    private int activeGunIndex = 0;
    private AudioSource audioSource;
    [SerializeField] AudioSource dashAudioSource;
    private SpriteRenderer spriteRenderer;
    private CameraFollow playerCamera;
    private bool hasRifle;
    private bool hasShotgun;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            crosshair.SetActive(false);
        }
        else
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

        if (dashBar != null)
        {
            dashBar.maxValue = dashCooldown;
            dashBar.value = dashCooldown;
        }

        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].gameObject.SetActive(i == activeGunIndex);
        }

        hasRifle = PlayerPrefs.GetInt("rifle", 0) == 1;
        hasShotgun = PlayerPrefs.GetInt("shotgun", 0) == 1;
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        crosshair.transform.position = mousePosition;

        facingRight = crosshair.transform.position.x > transform.position.x;

        if (facingRight && transform.localScale.x < 0 || !facingRight && transform.localScale.x > 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxis("Vertical");

        bool isWalking = dirX * moveSpeed != 0 || dirY * moveSpeed != 0;
        animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else if (audioSource.isPlaying) audioSource.Stop();

        if (dashBar != null)
        {
            dashBar.value = Mathf.Clamp(Time.time - lastDashTime, 0, dashCooldown);

            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
            {
                StartCoroutine(Dash());
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && guns.Length >= 1)
        {
            SwitchGun(0);
        }
        else if (hasRifle && Input.GetKeyDown(KeyCode.Alpha2) && guns.Length >= 2)
        {
            SwitchGun(1);
        }
        else if (hasShotgun && Input.GetKeyDown(KeyCode.Alpha3) && guns.Length >= 3)
        {
            SwitchGun(2);
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Vector2 movement = new Vector2(dirX, dirY).normalized * moveSpeed;
            rb.velocity = movement;
        }
    }

    private void LateUpdate()
    {
        if (guns.Length >= 1) RotateWeapon();
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

        ChangeAlpha(0.5f);

        Vector2 dashDirection = new Vector2(dirX, dirY).normalized;
        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        ChangeAlpha(1f);

        isDashing = false;
    }

    private void ChangeAlpha(float alphaValue)
    {
        Color color = spriteRenderer.color;
        color.a = alphaValue;
        spriteRenderer.color = color;
    }
}