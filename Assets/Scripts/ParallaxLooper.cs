using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ParallaxLooper : MonoBehaviour
{
    public float baseSpeed = 5f;       // Tốc độ cuộn của lớp
    public float tileWidth = 51.02f;     // Chiều dài 1 tile

    private Transform tileA;
    private Transform tileB;
    private float lastLoggedSpeed = -1f;
    private bool isGameOver = false;
    private bool isMapStopped = false;

    private int loopCount = 0;
    public ObstacleSpawner obstacleSpawner;
    public ItemSpawner itemSpawner;
    public GameObject dialoguePanel;
    public static bool globalMapStopped = false;
    void Start()
    {
        // Lấy tile con (giả sử tileA là con 0, tileB là con 1)
        tileA = transform.GetChild(0);
        tileB = transform.GetChild(1);

        if (obstacleSpawner == null) obstacleSpawner = FindAnyObjectByType<ObstacleSpawner>();
        if (itemSpawner == null) itemSpawner = FindAnyObjectByType<ItemSpawner>();
    }

    public void SetGameOver()
    {
        isGameOver = true;
    }

    void Update()
    {
        if (isGameOver || isMapStopped) return;
        float multiplier = MapSpeedController.Instance != null ? MapSpeedController.Instance.SpeedMultiplier : 1f;
        float speed = baseSpeed * multiplier;
        tileA.Translate(Vector3.left * speed * Time.deltaTime);
        tileB.Translate(Vector3.left * speed * Time.deltaTime);
        if (!Mathf.Approximately(speed, lastLoggedSpeed))
        {
            Debug.Log($"[Parallax] current speed đang dùng: {speed}");
            lastLoggedSpeed = speed;
        }


        // Nếu tileA ra khỏi màn → đặt nó ra sau tileB
        if (tileA.position.x <= -tileWidth)
        {
            tileA.position = new Vector3(tileB.position.x + tileWidth, tileA.position.y, tileA.position.z);
            SwapTiles();

            loopCount++;
            Debug.Log($"[Parallax] Đã hoàn thành vòng thứ: {loopCount}");
            if (loopCount >= 10)
            {
                //TriggerCutScene();
                StartCoroutine(PrepareStopMap());
            }
        }

        // Nếu tileB ra khỏi màn → đặt nó ra sau tileA
        if (tileB.position.x <= -tileWidth)
        {
            tileB.position = new Vector3(tileA.position.x + tileWidth, tileB.position.y, tileB.position.z);
            SwapTiles();
        }
    }

    void SwapTiles()
    {
        // Đảo thứ tự tileA, tileB nếu cần
        if (tileA.position.x > tileB.position.x)
        {
            var temp = tileA;
            tileA = tileB;
            tileB = temp;
        }
    }
    // THÊM: Hàm trigger CutScene sau 5 vòng
    //void TriggerCutScene()
    //{
    //    // Không set isGameOver=true để background tiếp tục cuộn
    //    Debug.Log("[Parallax] Đạt 5 vòng, dừng spawn và destroy vật cản/item, tiếp tục cuộn 5 giây trước khi chuyển CutScene");

    //    // Dừng spawn và destroy vật cản/item
    //    if (obstacleSpawner != null)
    //    {
    //        obstacleSpawner.StopSpawning();  // SỬA: Gọi StopSpawning thay SetGameOver
    //        StartCoroutine(obstacleSpawner.DestroyAllObstaclesCoroutine());
    //    }
    //    if (itemSpawner != null)
    //    {
    //        itemSpawner.StopSpawning();  // SỬA: Gọi StopSpawning thay SetGameOver
    //        StartCoroutine(itemSpawner.DestroyAllItemsCoroutine());
    //    }

    //    // Dọn bộ nhớ (nếu máy yếu)
    //    //System.GC.Collect();
    //    //Resources.UnloadUnusedAssets();

    //    // Load CutScene sau 5 giây (background vẫn cuộn)
    //    Invoke("LoadCutScene", 10f);
    //}
    IEnumerator PrepareStopMap()
    {
        Debug.Log("[Parallax] Đạt 10 vòng, dừng spawn và destroy vật cản/item");

        if (obstacleSpawner != null)
        {
            obstacleSpawner.StopSpawning();
            yield return StartCoroutine(obstacleSpawner.DestroyAllObstaclesCoroutine());
        }
        if (itemSpawner != null)
        {
            itemSpawner.StopSpawning();
            yield return StartCoroutine(itemSpawner.DestroyAllItemsCoroutine());
        }

        // Chờ 7 giây sau khi destroy hết
        yield return new WaitForSeconds(7f);

        StopMap();
    }
    void StopMap()
    {
        isMapStopped = true;
        globalMapStopped = true; // Dừng tất cả layers
        Debug.Log("[Parallax] Sau 7s, dừng map, dừng timer, hiện panel");

        // Dừng timer (giả sử GlobalDataManager có flag public bool isTimerRunning)
        if (GlobalDataManager.Instance != null)
        {
            GlobalDataManager.Instance.isTimerRunning = false;
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("DialoguePanel chưa assign!");
        }
    }

    //void LoadCutScene()
    //{
    //    isGameOver = true;  // Dừng cuộn trước khi load
    //    SceneManager.LoadScene("CutScene");
    //}
}


