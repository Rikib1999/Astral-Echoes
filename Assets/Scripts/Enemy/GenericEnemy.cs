using UnityEngine;

public class GenericEnemy : MonoBehaviour
{
    public Transform player;               // Reference to the player
    public float moveSpeed = 2f;           // Movement speed
    public float attackRange = 1.5f;       // Distance within which the enemy attacks
    public float detectionRange = 10f;     // Distance within which the enemy starts following the player
    public float attackCooldown = 2f;      // Time between attacks

    private Animator animator;             // Reference to the Animator component
    private bool isAttacking = false;      // If the enemy is currently attacking
    private float attackTimer = 0f;        // Timer to manage attack cooldown
    private Transform enemyTransform;      // Cached reference to enemy's transform

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;  
        }
        // Cache components
        animator = GetComponent<Animator>();
        enemyTransform = gameObject.transform;
    }

    private void Update()
    {
        if (player == null) return; // Exit if player is not assigned

        float distanceToPlayer = Vector3.Distance(enemyTransform.position, player.position);

        if (distanceToPlayer <= detectionRange && !isAttacking)
        {
            FollowPlayer(distanceToPlayer);
        }

        if (distanceToPlayer <= attackRange && attackTimer <= 0f)
        {
            AttackPlayer();
        }

        // Update attack cooldown timer
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void FollowPlayer(float distanceToPlayer)
    {
        // Play walk animation
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }

        // Move towards the player
        Vector3 direction = (player.position - enemyTransform.position).normalized;
        enemyTransform.position += direction * moveSpeed * Time.deltaTime;

        // Variables for scale
        Vector3 originalScale = enemyTransform.localScale; // Store the initial scale

        // Face the player
        if (direction.x > 0)
        {
            enemyTransform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Face right
        }
        else if (direction.x < 0)
        {
            enemyTransform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Face left
        }
    }

    private void AttackPlayer()
    {
        // Stop moving and play attack animation
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetTrigger("Attack");
        }

        isAttacking = true;
        attackTimer = attackCooldown;

        // Damage logic can be added here, e.g., calling a method on the player's health component

        // Simulate attack completion after animation ends (adjust time as per animation length)
        Invoke(nameof(ResetAttack), 1f); // 1 second is an example; match this to your animation length
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection and attack range in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
