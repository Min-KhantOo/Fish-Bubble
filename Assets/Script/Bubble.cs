using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;

    private Vector2 direction;
    private Fish owner;


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
            // Ignore the fish that shot the bubble
            if (fish == owner)
                return;


            fish.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}