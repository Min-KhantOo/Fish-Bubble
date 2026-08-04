using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject healthPickupPrefab;
    [SerializeField] private float minSpawnTime = 8f;
    [SerializeField] private float maxSpawnTime = 15f;

    [Header("Spawn Padding")]
    [SerializeField] private float sidePadding = 0.1f;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        ScheduleNextSpawn();
    }

    void ScheduleNextSpawn()
    {
        float randomTime = Random.Range(minSpawnTime, maxSpawnTime);
        Invoke(nameof(SpawnHealthItem), randomTime);
    }

    void SpawnHealthItem()
    {
        if (healthPickupPrefab != null && mainCam != null)
        {
            // Pick a random X position within camera bounds
            Vector3 minScreen = mainCam.ViewportToWorldPoint(new Vector3(sidePadding, 0, 0));
            Vector3 maxScreen = mainCam.ViewportToWorldPoint(new Vector3(1f - sidePadding, 0, 0));

            float randomX = Random.Range(minScreen.x, maxScreen.x);

            // Spawn just below the bottom of the screen so it floats up smoothly
            float spawnY = minScreen.y - 0.5f;

            Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

            Instantiate(healthPickupPrefab, spawnPosition, Quaternion.identity);
        }

        // Schedule next random spawn
        ScheduleNextSpawn();
    }
}