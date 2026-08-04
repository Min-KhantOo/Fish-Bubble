using UnityEngine;

public class Worm : Fish
{
    [Header("Worm Combat Settings")]
    [SerializeField] private float contactDamage = 10f;

    private Camera cam;

    private enum Edge
    {
        Bottom,
        Right,
        Top,
        Left
    }

    private Edge currentEdge = Edge.Bottom;

    void Start()
    {
        cam = Camera.main;

        if (cam != null)
        {
            Vector3 min = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));

            // Spawn at bottom-left corner with padding inherited from Fish
            transform.position = new Vector3(min.x + padding, min.y + padding, 0);
        }
    }

    void Update()
    {
        if (cam == null) return;

        Vector3 min = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        switch (currentEdge)
        {
            case Edge.Bottom:
                Move(Vector2.right);

                if (transform.position.x >= max.x - padding)
                    currentEdge = Edge.Right;
                break;

            case Edge.Right:
                Move(Vector2.up);

                if (transform.position.y >= max.y - padding)
                    currentEdge = Edge.Top;
                break;

            case Edge.Top:
                Move(Vector2.left);

                if (transform.position.x <= min.x + padding)
                    currentEdge = Edge.Left;
                break;

            case Edge.Left:
                Move(Vector2.down);

                if (transform.position.y <= min.y + padding)
                    currentEdge = Edge.Bottom;
                break;
        }
    }

    // Deal damage to player when touched
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
}