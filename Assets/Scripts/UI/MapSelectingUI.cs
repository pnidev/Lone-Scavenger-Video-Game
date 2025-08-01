using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleMapUIController : MonoBehaviour
{
    [Header("Panel map chọn")]
    public GameObject mapSelectingPanel;

    [Header("Button")]
    public Button openMapPanelButton;
    public Button returnButton;

void Start()
{
    // Ẩn panel map chọn ngay khi bắt đầu
    if (mapSelectingPanel != null)
        mapSelectingPanel.SetActive(false);

    // Gán sự kiện nút mở panel chọn map
    if (openMapPanelButton != null)
        openMapPanelButton.onClick.AddListener(OpenMapPanel);

    // Gán sự kiện nút return về Loading
    if (returnButton != null)
        returnButton.onClick.AddListener(OnReturnClicked);
}


    public void OpenMapPanel()
    {
        if (mapSelectingPanel != null)
            mapSelectingPanel.SetActive(true);
    }

    void OnReturnClicked()
    {
        PlayerPrefs.SetString("NextScene", "HomePage");
        SceneManager.LoadScene("Loading");
    }
}
