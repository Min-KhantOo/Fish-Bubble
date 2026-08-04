using UnityEngine;

public class CatFish : Fish
{
    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float normalSpeed = 2f;
    public float dashSpeed = 12f;

    [Header("Attack Settings")]
    public float attackRange = 5f;
    public float waitBeforeDash = 2f;
    public float dashDuration = 0.5f;
    public float dashDamage = 20f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip dashSound;

    private Vector2 randomDirection;
    private float randomTimer;
    private float findPlayerTimer;

    private float attackTimer;
    private float currentDashTime;

    private Vector2 dashDirection;

    private bool preparingDash;
    private bool dashing;

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
        // Handle missing or destroyed player
        if (player == null || !player.gameObject.activeInHierarchy)
        {
            // Reset dash states if player was lost mid-attack
            preparingDash = false;
            dashing = false;

            // Search for player occasionally instead of every frame
            findPlayerTimer -= Time.deltaTime;
            if (findPlayerTimer <= 0f)
            {
                FindPlayer();
                findPlayerTimer = 1f;
            }

            RandomMove();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange || preparingDash || dashing)
        {
            AttackPlayer();
        }
        else
        {
            RandomMove();
        }
    }

    void RandomMove()
    {
        speed = normalSpeed;

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

    void AttackPlayer()
    {
        // 1. Prepare Dash
        if (!preparingDash && !dashing)
        {
            preparingDash = true;
            attackTimer = waitBeforeDash;
        }

        // 2. Waiting before dash
        if (preparingDash)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0)
            {
                preparingDash = false;
                dashing = true;
                currentDashTime = dashDuration;

                if (player != null)
                {
                    dashDirection = (player.position - transform.position).normalized;
                }

                if (audioSource != null && dashSound != null)
                {
                    audioSource.PlayOneShot(dashSound);
                }
            }
        }

        // 3. Perform Dash
        if (dashing)
        {
            speed = dashSpeed;
            Move(dashDirection);

            currentDashTime -= Time.deltaTime;

            if (currentDashTime <= 0)
            {
                dashing = false;
                speed = normalSpeed; // Reset speed after dash finishes
                ChooseRandomDirection();
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // Deal extra damage if hitting player while dashing
        if (other.CompareTag("Player"))
        {
            PlayerFish playerScript = other.GetComponent<PlayerFish>();
            if (playerScript != null)
            {
                float damageToDeal = dashing ? dashDamage : 10f;
                playerScript.TakeDamage(damageToDeal);
            }
        }
        else
        {
            base.OnTriggerEnter2D(other);
        }
    }

    protected override void Die()
    {
        base.Die();
    }
}