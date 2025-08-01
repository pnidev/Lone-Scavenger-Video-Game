using UnityEngine;
using UnityEngine.UI; // nếu dùng Text thường
using TMPro;          // nếu dùng TextMeshPro
using UnityEngine.SceneManagement;

public class VictoryUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject victoryPanel;        // Panel chính
    public GameObject[] stars;             // Mảng sao (Star1, Star2, Star3)
    public Button continueButton;          // Nút "Continue"
    public Button backToMapButton;         // Nút "Back to Map"

    [Header("Optional Settings")]
    public int starsEarned = 0;            // Số sao đạt được (1–3)

    private void Start()
    {
        // Ẩn panel ban đầu
        victoryPanel.SetActive(false);

        // Gán sự kiện cho nút
        continueButton.onClick.AddListener(OnContinue);
        backToMapButton.onClick.AddListener(OnBackToMap);
    }

    // Hàm gọi để hiển thị Victory UI
    public void ShowVictoryPanel(int earnedStars)
    {
        Debug.Log("Đã gọi ShowVictoryPanel");
        starsEarned = Mathf.Clamp(earnedStars, 0, 3);
        victoryPanel.SetActive(true);

        // Bật đúng số sao
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < starsEarned);
        }
    }

    private void OnContinue()
    {
        Debug.Log("Continue to next level...");
        // Ví dụ: load màn tiếp theo
        SceneManager.LoadScene("Phase1Scene"); // sửa tên nếu cần
    }

    private void OnBackToMap()
    {
        Debug.Log("Return to map...");
        SceneManager.LoadScene("MapSelecting"); // sửa tên nếu cần
    }
}
