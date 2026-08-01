using UnityEngine;

public class PlayerFish : Fish
{
    [Header("Shooting")]
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private Transform shootPoint;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(x, y);

        // Move
        Move(direction);

        // Flip sprite left/right
        if (x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (x < 0)
        {
            spriteRenderer.flipX = true;
        }

        // Shoot bubble
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bubble = Instantiate(
            bubblePrefab,
            shootPoint.position,
            shootPoint.rotation
        );

        Bubble bubbleScript = bubble.GetComponent<Bubble>();

        if (bubbleScript != null)
        {
            bubbleScript.SetOwner(this);

            // Shoot left or right depending on facing
            Vector2 shootDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

            bubbleScript.SetDirection(shootDirection);
        }
    }
}