using UnityEngine;

public class CatFish : Fish
{
    public Transform player;

    [Header("Movement")]
    public float normalSpeed = 2f;
    public float dashSpeed = 12f;

    [Header("Attack")]
    public float attackRange = 5f;
    public float waitBeforeDash = 2f;
    public float dashDuration = 0.5f;


    private Vector2 randomDirection;
    private float randomTimer;

    private float attackTimer;
    private Vector2 dashDirection;

    private bool preparingDash;
    private bool dashing;


    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        ChooseRandomDirection();
    }


    void Update()
    {
        if (player == null)
            return;


        float distance = Vector2.Distance(
            transform.position,
            player.position
        );


        // Player is close
        if (distance <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            // Normal random swimming
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

        randomTimer = Random.Range(1f, 3f);
    }


    void AttackPlayer()
    {
        // Wait before dash
        if (!preparingDash && !dashing)
        {
            preparingDash = true;

            attackTimer = waitBeforeDash;

            Debug.Log("CatFish preparing attack");
        }


        if (preparingDash)
        {
            attackTimer -= Time.deltaTime;


            if (attackTimer <= 0)
            {
                preparingDash = false;

                dashing = true;

                dashDirection =
                    (player.position - transform.position).normalized;
            }
        }


        if (dashing)
        {
            speed = dashSpeed;

            Move(dashDirection);


            dashDuration -= Time.deltaTime;


            if (dashDuration <= 0)
            {
                dashing = false;

                dashDuration = 0.5f;

                ChooseRandomDirection();
            }
        }
    }
}