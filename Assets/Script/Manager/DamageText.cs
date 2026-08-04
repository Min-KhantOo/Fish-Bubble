using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("Movement & Lifetime")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float lifeTime = 0.8f;

    [Header("Fade Effect")]
    [SerializeField] private bool fadeOut = true;

    private TMP_Text text;
    private Color originalColor;
    private float timer;
    private RectTransform rectTransform;

    void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();

        if (text != null)
        {
            originalColor = text.color;
        }

        // Force canvas component on this popup to render over all 2D sprites
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 100;
        }
    }

    void Update()
    {
        // Float upward smoothly in 2D space
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        timer += Time.deltaTime;

        // Smoothly fade out alpha color over lifetime
        if (fadeOut && text != null)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / lifeTime);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }

        // Destroy when lifetime ends
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(string message, Color textColor)
    {
        if (text == null) text = GetComponentInChildren<TMP_Text>();

        if (text != null)
        {
            text.text = message;
            text.color = textColor;
            originalColor = textColor;
        }
    }
}