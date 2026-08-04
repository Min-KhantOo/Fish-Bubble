using UnityEngine;
using UnityEngine.UI;

public class HealthBarColor : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;

    private void Update()
    {
        float healthPercent = healthSlider.value / healthSlider.maxValue;

        // Smooth color from Red -> Yellow -> Green
        if (healthPercent > 0.5f)
        {
            fillImage.color = Color.Lerp(Color.yellow, Color.green, (healthPercent - 0.5f) * 2f);
        }
        else
        {
            fillImage.color = Color.Lerp(Color.red, Color.yellow, healthPercent * 2f);
        }
    }
}