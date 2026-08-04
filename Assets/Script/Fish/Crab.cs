using UnityEngine;

public class Crab : Fish
{
    [Header("Crab Settings")]
    [SerializeField] private float contactDamage = 15f;
    [SerializeField] private float edgePadding = 0.05f;

    [Header("Facing Direction Setup")]
    [Tooltip("Check this if your crab sprite naturally faces LEFT in the PNG file.")]
    [SerializeField] private bool spriteFacesLeftByDefault = false;

    private int direction = 1; // 1 = Right, -1 = Left
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        mainCamera = Camera.main;

        // Apply correct initial facing orientation
        UpdateSpriteFacing();
    }

    void Update()
    {
        // Move left or right along the floor/edge
        Move(new Vector2(direction, 0));

        if (mainCamera == null) return;

        // Check screen boundaries in viewport coordinates
        Vector3 view = mainCamera.WorldToViewportPoint(transform.position);

        // Turn around at left edge
        if (view.x <= edgePadding && direction < 0)
        {
            direction = 1;
            UpdateSpriteFacing();
        }
        // Turn around at right edge
        else if (view.x >= 1f - edgePadding && direction > 0)
        {
            direction = -1;
            UpdateSpriteFacing();
        }
    }

    // Deal damage to the player when touched
    protected override void OnTriggerEnter2D(Collider2D other)
    {
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

    private void UpdateSpriteFacing()
    {
        if (spriteRenderer != null)
        {
            // Set flipX based on direction and original sprite orientation
            if (spriteFacesLeftByDefault)
            {
                spriteRenderer.flipX = (direction > 0);
            }
            else
            {
                spriteRenderer.flipX = (direction < 0);
            }
        }
    }
}