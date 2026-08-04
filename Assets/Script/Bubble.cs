using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Bubble Settings")]
    public float speed = 10f;
    public float damage = 20f;
    public float lifeTime = 3f;

    private Vector2 direction;
    private Fish owner;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetOwner(Fish fish)
    {
        owner = fish;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Fish fish = other.GetComponent<Fish>();

        if (fish != null)
        {
            // Do not hit the player/fish that shot this bubble
            if (fish == owner)
                return;

            // Deal damage (triggers Fish.TakeDamage & ShowDamageText)
            fish.TakeDamage(damage);

            // Destroy bubble
            Destroy(gameObject);
        }
    }
}