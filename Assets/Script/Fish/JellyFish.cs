using UnityEngine;

public class JellyFish : Fish
{
    [Header("JellyFish Combat Settings")]
    [SerializeField] private float contactDamage = 15f;

    private Vector2 direction;
    private float timer;

    void Start()
    {
        ChooseDirection();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            ChooseDirection();
        }

        Move(direction);
    }

    void ChooseDirection()
    {
        direction = Random.insideUnitCircle.normalized;
        timer = Random.Range(1f, 3f);
    }

    // Deal damage to the player when touched
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // Call base logic (if any)
        base.OnTriggerEnter2D(other);

        if (other.CompareTag("Player"))
        {
            PlayerFish player = other.GetComponent<PlayerFish>();
            if (player != null)
            {
                player.TakeDamage(contactDamage);
            }
        }
    }

    protected override void Die()
    {
        // Calls Fish.Die() which adds score and destroys the object
        base.Die();
    }
}