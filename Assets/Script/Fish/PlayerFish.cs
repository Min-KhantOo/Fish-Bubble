using TMPro;
using UnityEngine;

public class PlayerFish : Fish
{
    [Header("Shooting")]
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private Transform shootPoint;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shotSound;

    private SpriteRenderer playerSpriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(x, y);

        Move(direction);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bubblePrefab == null || shootPoint == null)
            return;

        GameObject bubble = Instantiate(
            bubblePrefab,
            shootPoint.position,
            shootPoint.rotation
        );

        if (audioSource != null && shotSound != null)
        {
            audioSource.PlayOneShot(shotSound);
        }

        Bubble bubbleScript = bubble.GetComponent<Bubble>();

        if (bubbleScript != null)
        {
            bubbleScript.SetOwner(this);

            Vector2 shootDirection = (playerSpriteRenderer != null && playerSpriteRenderer.flipX)
                ? Vector2.left
                : Vector2.right;

            bubbleScript.SetDirection(shootDirection);
        }
    }

    // Handles damage and shows RED negative text 
    public override void TakeDamage(float damage)
    {
        ShowDamageText(-damage, Color.red);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerTakeDamage(damage);
        }
    }

    // Handles healing and shows GREEN positive text (+25)
    public void Heal(float amount)
    {
        ShowDamageText(amount, Color.green);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.HealPlayer(amount);
        }
    }

    protected override void Die()
    {
        Debug.Log("Player Fish Died!");
        gameObject.SetActive(false);
    }
}