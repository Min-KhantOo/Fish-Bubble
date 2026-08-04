using TMPro;
using UnityEngine;

public abstract class Fish : MonoBehaviour
{
    [Header("Fish Stats")]
    public float health = 100f;
    public float speed = 5f;

    [Header("Border")]
    public float padding = 0.5f;

    [Header("Damage Text")]
    public GameObject damageTextPrefab;

    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Move(Vector3 direction)
    {
        transform.position += direction * speed * Time.deltaTime;

        // Face movement direction
        if (spriteRenderer != null)
        {
            if (direction.x > 0.01f)
            {
                spriteRenderer.flipX = false;
            }
            else if (direction.x < -0.01f)
            {
                spriteRenderer.flipX = true;
            }
        }

        KeepInsideCamera();
    }

    void KeepInsideCamera()
    {
        if (mainCamera == null)
            return;

        Vector3 pos = transform.position;

        Vector3 min = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        pos.x = Mathf.Clamp(pos.x, min.x + padding, max.x - padding);
        pos.y = Mathf.Clamp(pos.y, min.y + padding, max.y - padding);
        pos.z = 0;

        transform.position = pos;
    }

    //=========================
    // DAMAGE LOGIC
    //=========================

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        // Pass Red color for enemy damage text popup
        ShowDamageText(-damage, Color.red);

        Debug.Log(gameObject.name + " HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }
    public void ShowDamageText(float amount, Color textColor = default)
    {
        if (damageTextPrefab == null)
        {
            Debug.LogWarning(gameObject.name + " missing Damage Text Prefab in Inspector!");
            return;
        }

        if (textColor == default)
        {
            textColor = Color.red;
        }

        // Spawn point above current fish position
        Vector3 spawnPosition = transform.position + new Vector3(0f, 0.8f, 0f);

        // Spawn directly in World Space (Quaternion.identity prevents flipping with player)
        GameObject textObj = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity);

        DamageText damageTextScript = textObj.GetComponentInChildren<DamageText>();

        if (damageTextScript != null)
        {
            bool isHealing = (amount > 0);
            string prefix = isHealing ? "+" : "-";
            string message = prefix + Mathf.RoundToInt(Mathf.Abs(amount));

            damageTextScript.Setup(message, textColor);
        }
    }

    //=========================
    // COLLISION LOGIC
    //=========================

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!CompareTag("Player") && other.CompareTag("Player"))
        {
            PlayerFish player = other.GetComponent<PlayerFish>();
            if (player != null)
            {
                player.TakeDamage(10f); // Calls PlayerFish.TakeDamage cleanly once
            }
        }
    }

    public virtual void Attack()
    {
        Debug.Log("Fish Attack");
    }

    //=========================
    // DEATH LOGIC
    //=========================

    protected virtual void Die()
    {
        if (!CompareTag("Player") && GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(10);
        }

        Destroy(gameObject);
    }
}