using UnityEngine;

public class Crab : Fish
{
    private int direction = 1;

    public float edgePadding = 0.05f;

    private SpriteRenderer spriteRenderer;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        // Move left or right only
        Move(new Vector2(direction, 0));


        // Check screen edge
        Vector3 view = Camera.main.WorldToViewportPoint(transform.position);


        // Left edge
        if (view.x <= edgePadding)
        {
            direction = 1;
            Flip();
        }


        // Right edge
        if (view.x >= 1 - edgePadding)
        {
            direction = -1;
            Flip();
        }
    }


    void Flip()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}