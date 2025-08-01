using UnityEngine;
using UnityEngine.SceneManagement;  // Để check scene name

public class GlobalDataManager : MonoBehaviour
{
    public static GlobalDataManager Instance;

    public int currentHearts = 3;         // Máu (trái tim)
    public int currentEnergyBars = 0;     // Năng lượng (ô vàng)
    public int currentLevel = 1;          // Level
    public float elapsedTime = 0f;        // Thời gian timer


    [Header("CutScene Settings")]
    public string cutSceneName = "CutScene";  // Tên scene cutscene (chỉnh nếu khác)
    public bool isTimerRunning = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Giữ qua scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;  // Lấy tên scene hiện tại
        if (currentScene != cutSceneName && isTimerRunning)  // Nếu KHÔNG phải cutscene → đếm timer
        {
            elapsedTime += Time.deltaTime;
        }
        // Debug.Log("Timer: " + elapsedTime + "s, Scene: " + currentScene);  // Optional trace
    }
    public int CalculateStarRating()
    {
        if (elapsedTime <= 15f && currentEnergyBars >= 25)
            return 3;
        else if (elapsedTime <= 30f && currentEnergyBars >= 15)
            return 2;
        else
            return 1;
    }
}