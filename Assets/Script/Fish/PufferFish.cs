using UnityEngine;

public class PufferFish : Fish
{
    [Header("Attack Settings")]
    public float attackDistance = 1.5f;
    public float attackCooldown = 1f;
    public float attackDamage = 15f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip biteSound;

    [Header("Target")]
    public Transform player; // Made public to resolve protection level issues

    private float attackTimer;
    private bool isDead = false;

    // Random movement variables
    private Vector2 randomDirection;
    private float randomTimer;
    private float findPlayerTimer;

    void Start()
    {
        FindPlayer();
        ChooseRandomDirection();
    }

    // Public setter called by EnemySpawner
    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null && playerObject.activeInHierarchy)
        {
            player = playerObject.transform;
        }
        else
        {
            player = null;
        }
    }

    void Update()
    {
        if (isDead)
            return;

        // Check if player is missing, destroyed, or inactive
        if (player == null || !player.gameObject.activeInHierarchy)
        {
            // Search for player occasionally (every 1 second) instead of every frame
            findPlayerTimer -= Time.deltaTime;
            if (findPlayerTimer <= 0)
            {
                FindPlayer();
                findPlayerTimer = 1f;
            }

            // If still no active player (e.g. Main Menu), do smooth random swimming
            RandomMove();
            return;
        }

        // --- PLAYER EXISTS: CHASE & ATTACK ---
        Vector3 direction = (player.position - transform.position).normalized;
        Move(direction);

        attackTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        // Attack when close
        if (distance < attackDistance && attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    void RandomMove()
    {
        randomTimer -= Time.deltaTime;

        if (randomTimer <= 0)
        {
            ChooseRandomDirection();
        }

        Move(randomDirection);
    }

    void ChooseRandomDirection()
    {
        randomDirection = Random.insideUnitCircle.normalized;
        randomTimer = Random.Range(1.5f, 3.5f);
    }

    public override void Attack()
    {
        if (player != null)
        {
            PlayerFish playerScript = player.GetComponent<PlayerFish>();

            if (playerScript != null)
            {
                playerScript.TakeDamage(attackDamage);
            }
        }

        if (audioSource != null && biteSound != null)
        {
            audioSource.PlayOneShot(biteSound);
        }
    }

    protected override void Die()
    {
        isDead = true;
        base.Die();
    }
}