using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [Header("Player")]
    public float maxHealth = 100f;
    private float currentHealth;


    [Header("UI")]
    public TMP_Text scoreText;
    public Slider healthSlider;
    public GameObject gameOverPanel;


    private int score = 0;


    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        scoreText.text = "SCORE: 0";

        gameOverPanel.SetActive(false);
    }


    public void AddScore(int amount)
    {
        score += amount;

        scoreText.text = "SCORE: " + score;
    }


    public void PlayerTakeDamage(float damage)
    {
        currentHealth -= damage;

        healthSlider.value = currentHealth;


        if (currentHealth <= 0)
        {
            GameOver();
        }
    }


    void GameOver()
    {
        gameOverPanel.SetActive(true);

        Time.timeScale = 0;
    }
}