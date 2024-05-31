using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform[] guns; // Array to store references to all guns
    [SerializeField] Transform crosshair;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float dirX = 0;
    [SerializeField] float dirY = 0;
    bool facingRight = true;
    Animator animator;
    private Vector3 lastMousePosition;
    private int activeGunIndex = 0; // Index of the currently active gun

    // Start is called before the first frame update
    void Start()
    {
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
        crosshair.position = mousePosition;

        // Determine if the mouse has moved left or right
        if (crosshair.position.x > transform.position.x)
        {
            facingRight = true;
        }
        else if (crosshair.position.x < transform.position.x)
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
        if (dirX*moveSpeed != 0 || dirY* moveSpeed != 0) animator.SetBool("isWalking",true);
        else animator.SetBool("isWalking",false);
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
        // Move the player based on input
        Vector2 movement = new Vector2(dirX, dirY).normalized * moveSpeed;
        rb.velocity = movement;
    }

    private void LateUpdate()
    {
        RotateWeapon();
    }

    private void RotateWeapon()
    {
        // Get the position of the crosshair
        Vector3 targetPosition = crosshair.position;

        // Get the direction from the active gun's muzzle to the crosshair
        Vector3 direction = targetPosition - guns[activeGunIndex].position;

        // Calculate the rotation angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Flip the gun along the X axis if it's on the left side
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
}
