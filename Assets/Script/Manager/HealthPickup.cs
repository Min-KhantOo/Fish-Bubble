using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Healing Settings")]
    [SerializeField] private float healAmount = 25f;
    [SerializeField] private float floatSpeed = 1.2f;
    [SerializeField] private float lifeTime = 10f;

    [Header("Audio")]
    [SerializeField] private AudioClip healSound;

    private float timer;
    private bool isCollected = false;

    void Update()
    {
        if (isCollected) return;

        // Float upward
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;

            PlayerFish player = other.GetComponent<PlayerFish>();

            // 1. Heal player & show green floating text (+25)
            if (player != null)
            {
                player.Heal(healAmount);
            }
            else if (GameManager.Instance != null)
            {
                GameManager.Instance.HealPlayer(healAmount);
            }

            // 2. Play sound independently in world space
            if (healSound != null)
            {
                AudioSource.PlayClipAtPoint(healSound, Camera.main.transform.position);
            }

            // 3. Destroy pickup item
            Destroy(gameObject);
        }
    }
}