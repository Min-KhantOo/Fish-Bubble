using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float padding = 0.5f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("EnemySpawner has no enemy prefabs assigned!");
            return;
        }

        GameObject enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector3 min = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        Vector3 spawnPos = Vector3.zero;

        int side = Random.Range(0, 4);

        switch (side)
        {
            // Left
            case 0:
                spawnPos = new Vector3(
                    min.x + padding,
                    Random.Range(min.y + padding, max.y - padding),
                    0);
                break;

            // Right
            case 1:
                spawnPos = new Vector3(
                    max.x - padding,
                    Random.Range(min.y + padding, max.y - padding),
                    0);
                break;

            // Bottom
            case 2:
                spawnPos = new Vector3(
                    Random.Range(min.x + padding, max.x - padding),
                    min.y + padding,
                    0);
                break;

            // Top
            case 3:
                spawnPos = new Vector3(
                    Random.Range(min.x + padding, max.x - padding),
                    max.y - padding,
                    0);
                break;
        }

        GameObject newEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);

        // Find the player and assign reference to spawned enemies
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PufferFish puffer = newEnemy.GetComponent<PufferFish>();
            if (puffer != null)
            {
                puffer.SetPlayer(player.transform); // Fixed line 76: Uses SetPlayer public method
            }

            CatFish catfish = newEnemy.GetComponent<CatFish>();
            if (catfish != null)
            {
                catfish.player = player.transform;
            }
        }
    }
}