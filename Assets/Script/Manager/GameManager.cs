using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Reference")]
    [SerializeField] private PlayerFish player;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Controls")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Pause UI")]
    [SerializeField] private GameObject pauseMenuPanel; // Added variable declaration

    private int score;
    private bool isGameOver;
    private bool isPaused; // Added variable declaration

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;

        score = 0;
        currentHealth = maxHealth;
        isGameOver = false;
        isPaused = false;

        // Cache player reference if not set in Inspector
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerFish>();
        }

        if (player != null)
        {
            player.health = maxHealth;
        }

        UpdateScoreUI();
        UpdateHealthUI();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Toggle Pause with Escape or P key
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isGameOver) return; // Don't allow pausing on game over screen

            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    //=========================
    // SCORE
    //=========================

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE : {score}";
        }
    }

    public int GetScore()
    {
        return score;
    }

    //=========================
    // HEALTH
    //=========================

    public void PlayerTakeDamage(float damage)
    {
        if (isGameOver) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (player != null)
        {
            player.health = currentHealth;
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void HealPlayer(float amount)
    {
        if (isGameOver) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (player != null)
        {
            player.health = currentHealth;
        }

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"HP: {Mathf.Max(0, Mathf.RoundToInt(currentHealth))} / {maxHealth}";
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    //=========================
    // GAME OVER
    //=========================

    private void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = $"{score}";
        }

        Time.timeScale = 0f;
    }

    //=========================
    // BUTTONS
    //=========================

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //=========================
    // PAUSE LOGIC
    //=========================

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}