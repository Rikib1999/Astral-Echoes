using UnityEngine;

/// <summary>
/// Script for the basic enemies. Getting player position in the start and chasing the player and when close attack it.
/// Design to work with all enemies bcecause we can adjust speed range etc
/// </summary>
public class GenericEnemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 1.5f;
    public float detectionRange = 10f;
    public float attackCooldown = 2f;
    public int damage = 10;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isAttacking = false;
    private float attackTimer = 0f;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !isAttacking)
        {
            FollowPlayer(distanceToPlayer);
        }

        if (distanceToPlayer <= attackRange && attackTimer <= 0f)
        {
            AttackPlayer();
        }

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }
    /// <summary>
    /// Following player and facing its way
    /// </summary>
    /// <param name="distanceToPlayer"></param>
    private void FollowPlayer(float distanceToPlayer)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }

        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        FacePlayer(direction);
    }

    private void FacePlayer(Vector3 direction)
    {
        Vector3 originalScale = transform.localScale;
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }
    /// <summary>
    /// Attacking player when in range
    /// </summary>
    private void AttackPlayer()
    {
        rb.velocity = Vector3.zero;

        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetTrigger("Attack");
        }

        PlayerLogic playerLogic = player.GetComponent<PlayerLogic>();

        if (playerLogic != null)
        {
            playerLogic.damage(damage);
        }

        isAttacking = true;
        attackTimer = attackCooldown;

        Invoke(nameof(ResetAttack), 1f);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }
    /// <summary>
    /// Gizmos to visualize the range and roaming range
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
