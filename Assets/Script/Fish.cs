using UnityEngine;

public abstract class Fish : MonoBehaviour
{
    [Header("Fish Stats")]
    public float health = 100f;
    public float speed = 5f;

    [Header("Border")]
    public float padding = 0.5f;

    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    public virtual void Move(Vector3 direction)
    {
        transform.position += direction * speed * Time.deltaTime;

        KeepInsideCamera();
    }

    void KeepInsideCamera()
    {
        Vector3 pos = transform.position;

        Vector3 min = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        pos.x = Mathf.Clamp(pos.x, min.x + padding, max.x - padding);
        pos.y = Mathf.Clamp(pos.y, min.y + padding, max.y - padding);

        pos.z = 0;

        transform.position = pos;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log(gameObject.name + " HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Attack()
    {
        Debug.Log("Fish Attack");
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}