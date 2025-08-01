using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 4f;
    //public Vector2 spawnPos = new Vector2(20f, -3.5f);
    public float groundSpawnY = -3f;  // Vị trí thấp
    public float airSpawnY = 0.5f;      // Vị trí cao
    public float airSpawnChance = 0.5f; // 30% spawn vật cản trên

    public int maxObstacles = 5;

    private float timer;
    private float nextSpawnTime;
    private List<GameObject> spawnedObstacles = new List<GameObject>();
    private bool isGameOver = false;
    private bool spawningStopped = false;



    void Start()
    {
        SetNextSpawnTime();
    }
    public void SetGameOver()
    {
        isGameOver = true;
    }

    void Update()
    {
        if (isGameOver || spawningStopped) return;
        timer += Time.deltaTime;
        if (timer >= nextSpawnTime)
        {
            if (spawnedObstacles.Count < maxObstacles)
            {
                SpawnObstacle();
            }

            timer = 0f;
            SetNextSpawnTime();
        }


        spawnedObstacles.RemoveAll(o => o == null);
    }
    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }


    void SpawnObstacle()
    {

        bool spawnAir = Random.value < airSpawnChance;


        GameObject selectedPrefab = null;
        int attempts = 0;

        while (selectedPrefab == null && attempts < 10)
        {
            GameObject candidate = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            ObstacleBase baseScript = candidate.GetComponent<ObstacleBase>();

            if (baseScript == null || baseScript.isAirObstacle != spawnAir)
            {
                attempts++;
                continue;
            }

            if (spawnAir && baseScript.obstacleType == ObstacleBase.ObstacleType.Explosive)
            {
                attempts++;
                continue;
            }

            selectedPrefab = candidate;
        }

        float spawnY = groundSpawnY;
        ObstacleBase selectedBaseScript = selectedPrefab.GetComponent<ObstacleBase>();
        if (selectedBaseScript != null && selectedBaseScript.isAirObstacle && !selectedBaseScript.forceGroundYSpawn)
        {
            spawnY = airSpawnY;
        }

        Vector2 spawnPosition = new Vector2(20f, spawnY);
        GameObject newObstacle = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        spawnedObstacles.Add(newObstacle);

    }
    // THÊM: Hàm để dừng spawn (set spawningStopped)
    public void StopSpawning()
    {
        spawningStopped = true;
        Debug.Log("[ObstacleSpawner] Đã dừng spawn vật cản.");
    }

    public void DestroyAllObstacles()
    {
        foreach (var obs in spawnedObstacles)
        {
            if (obs != null)
            {
                Destroy(obs);
            }
        }
        spawnedObstacles.Clear();
        Debug.Log("[ObstacleSpawner] Đã destroy tất cả vật cản.");
    }
    // Trong ItemSpawner.cs
    // Trong ObstacleSpawner.cs, thêm hàm sau:
    public IEnumerator DestroyAllObstaclesCoroutine()
    {
        var obstacles = GameObject.FindGameObjectsWithTag("Obstacle"); // Thay "Obstacle" bằng tag của obstacles nếu khác
        int batchSize = 5;
        for (int i = 0; i < obstacles.Length; i += batchSize)
        {
            for (int j = 0; j < batchSize && (i + j) < obstacles.Length; j++)
            {
                Destroy(obstacles[i + j]);
            }
            yield return null; // Chờ frame tiếp theo
        }
        Debug.Log("[ObstacleSpawner] Đã destroy tất cả obstacles dần dần.");
    }
}

