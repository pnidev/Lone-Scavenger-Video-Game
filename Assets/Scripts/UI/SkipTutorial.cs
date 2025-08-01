using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipTutorial : MonoBehaviour
{
    [Header("Button")]
    public Button SkipButton;

    void Start()
    {
        // Gán sự kiện nút return về Loading
        if (SkipButton != null)
            SkipButton.onClick.AddListener(OnReturnClicked);
    }

    void OnReturnClicked()
    {
        PlayerPrefs.SetString("NextScene", "Phase1Scene");
        SceneManager.LoadScene("Loading");
    }
}