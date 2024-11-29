using UnityEngine;

public enum Enemy { walking, attacking, dead }

public class EnemyAI : MonoBehaviour
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
    private float timeBtwShots;

    public float shootingTime;
    public GameObject bullet;
    public Transform firePoint;
    public float shootrange = 5f;


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
            StopAndShoot();
        }
        else
        {
            Roam();
        }

        if (player)
        {
            DetectPlayer();
        }
    }

    private void FixedUpdate()
    {
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

    void StopAndShoot()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < shootrange && player != null)
        {
            ShootPlayer();
        }
        else
        {
            enemyState = Enemy.walking;
            FlipTowardsPlayer();
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    void ShootPlayer()
    {
        enemyState = Enemy.attacking;
        timeBtwShots += Time.deltaTime;

        Vector2 hm = transform.localScale;

        float fasterBullets = hm.x;

        shootingTime = fasterBullets - 0.2f;

        if (timeBtwShots >= shootingTime)
        {
            timeBtwShots = 0;
            Instantiate(bullet, firePoint.position, Quaternion.identity);
        }
    }

    void AttackPlayer()
    {
        enemyState = Enemy.attacking;
        FlipTowardsPlayer();
        Debug.Log("Attack!");
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