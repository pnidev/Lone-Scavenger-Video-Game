using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs; // Kéo Item_Boost và Item_LevelUp vào đây
    public float minY = -3.5f;         // Y thấp nhất
    public float maxY = -0.1f;          // Y cao nhất
    public float minSpawnInterval = 5f; // Thời gian spawn min
    public float maxSpawnInterval = 7f; // Thời gian spawn max (random để đa dạng)
    public float spawnX = 20f;       // X ngoài màn phải, đồng bộ với ObstacleSpawner

    private float timer;
    private float nextSpawnTime;
    private bool isGameOver = false;

    private List<GameObject> spawnedItems = new List<GameObject>();
    private bool spawningStopped = false;
    void Start()
    {
        SetNextSpawnTime(); // Set thời gian spawn đầu tiên

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
            SpawnItem();
            timer = 0f;
            SetNextSpawnTime();
        }
        spawnedItems.RemoveAll(o => o == null);
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void SpawnItem()
    {
        if (itemPrefabs.Length == 0) return;

        GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)]; // Random Boost hoặc LevelUp

        bool spawned = false;
        for (int attempt = 0; attempt < 20; attempt++) // 20 lần thử
        {
            float spawnY = Random.Range(minY, maxY);
            Vector2 spawnPos = new Vector2(spawnX, spawnY);

            float avoidRadius = 3f; // Default tránh overlap trực tiếp

            // Điều chỉnh radius dựa trên Y để tránh kế bên trên/dưới
            if (spawnY > 0f) avoidRadius = 4f;  // Y cao (air) tránh xa hơn
            else if (spawnY < -1f) avoidRadius = 4f;  // Y thấp (ground) tránh xa hơn

            Collider2D hit = Physics2D.OverlapCircle(spawnPos, avoidRadius, LayerMask.GetMask("Obstacle"));
            if (hit == null)
            {
                // SỬA: Check nhiều raycast để đảm bảo không có obstacle chắn đường (kế bên)
                bool pathClear = true;
                float[] yOffsets = { 0f, 0.5f, -0.5f };  // Check Y chính + trên/dưới
                foreach (float offset in yOffsets)
                {
                    Vector2 rayStart = new Vector2(spawnX - 15f, spawnY + offset);  // SỬA: Tăng distance lên 15f
                    RaycastHit2D rayHit = Physics2D.Raycast(rayStart, Vector2.right, 15f, LayerMask.GetMask("Obstacle"));
                    if (rayHit.collider != null)
                    {
                        pathClear = false;
                        Debug.Log($"[ItemSpawn Debug] Skip vị trí {spawnPos}: Có obstacle chắn tại {rayHit.point} (offset Y {offset})");  // THÊM: Log debug lý do skip
                        break;
                    }
                }

                if (pathClear)
                {
                    GameObject item = Instantiate(prefab, spawnPos, Quaternion.identity);
                    ItemMoverAndFloat mover = item.GetComponent<ItemMoverAndFloat>();  // SỬA: Thay FloatingItem bằng ItemMoverAndFloat, và đổi tên variable floating → mover
                    if (mover != null) mover.floatSpeed = 2f; // Có thể adjust
                    Debug.Log($"[ItemSpawn Debug] Spawn thành công tại {spawnPos}");  // THÊM: Log vị trí spawn ok
                    spawned = true;
                    break;
                }
            }
        }

        if (!spawned)
        {
            Debug.Log("Không tìm vị trí spawn item an toàn, bỏ qua lần này.");
            // Fallback nếu cần (comment sẵn)
            // Vector2 fallbackPos = new Vector2(spawnX, 0f);
            // GameObject item = Instantiate(prefab, fallbackPos, Quaternion.identity);
        }
    }
    // THÊM: Hàm để dừng spawn (set spawningStopped)
    public void StopSpawning()
    {
        spawningStopped = true;
        Debug.Log("[ItemSpawner] Đã dừng spawn item.");
    }

    public void DestroyAllItems()
    {
        foreach (var item in spawnedItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        spawnedItems.Clear();
        Debug.Log("[ItemSpawner] Đã destroy tất cả items.");
    }
    public IEnumerator DestroyAllItemsCoroutine()
    {
        int batchSize = 5;
        for (int i = 0; i < spawnedItems.Count; i += batchSize)
        {
            for (int j = 0; j < batchSize && (i + j) < spawnedItems.Count; j++)
            {
                if (spawnedItems[i + j] != null)
                    Destroy(spawnedItems[i + j]);
            }
            yield return null; // chờ frame tiếp theo
        }
        spawnedItems.Clear();
        Debug.Log("[ItemSpawner] Đã destroy tất cả items dần dần.");
    }

}