using UnityEngine;

public class EnemyAIPunch : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float roamRadius = 10f;
    public float attackCooldown = 1f;
    public GameObject player;
    public LayerMask playerLayer;
    public int playerDamage = 20;

    private Vector2 roamPosition;
    private Vector2 startPosition;
    private float lastAttackTime = 0f;
    private bool isChasing = false;
    private Animator animator;
    private Enemy enemyState;


    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        enemyState = Enemy.walking;
        player = GameObject.FindGameObjectWithTag("Player");
        SetNewRoamPosition();
    }

    void Update()
    {
        if (player && isChasing)
        {
            ChasePlayer();
            //StopAndShoot();
        }
        else
        {
            Roam();
        }

        if(player)
        {
            DetectPlayer();
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log(enemyState);
        if (enemyState == Enemy.walking)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        else if (enemyState == Enemy.attacking)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
        }
    }

    void Roam()
    {
        transform.position = Vector2.MoveTowards(transform.position, roamPosition, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, roamPosition) < 0.1f)
        {
            SetNewRoamPosition();
        }
    }

    void SetNewRoamPosition()
    {
        float roamX = Random.Range(startPosition.x - roamRadius, startPosition.x + roamRadius);
        float roamY = Random.Range(startPosition.y - roamRadius, startPosition.y + roamRadius);
        roamPosition = new Vector2(roamX, roamY);
    }

    void DetectPlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            isChasing = true;
        }
    }

    void ChasePlayer()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < attackRange && player!=null)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Debug.Log("AttackPlayer");
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            enemyState = Enemy.walking;
            FlipTowardsPlayer();
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }
    /// <summary>
    /// Attack player
    /// </summary>
    void AttackPlayer()
    {
        enemyState = Enemy.attacking;
        FlipTowardsPlayer();
        PunchDamage(player);

        Debug.Log("Attack!");
    }

    /// <summary>
    /// Give him damage throught the playerLogic
    /// </summary>
    /// <param name="player"></param>
    void PunchDamage(GameObject player)
    {
        PlayerLogic playerLogic = player.GetComponent<PlayerLogic>();

        if (playerLogic != null)
        {
            playerLogic.damage(5);
        }
        else
        {
            Debug.Log("PlayerLogic is null");
        }
    }

    void FlipTowardsPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerLogic>().damage(playerDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}