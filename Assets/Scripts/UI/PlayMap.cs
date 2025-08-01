using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayMap : MonoBehaviour
{
    [Header("Button")]
    public Button PlayButton;

    void Start()
    {
        // Gán sự kiện nút return về Loading
        if (PlayButton != null)
            PlayButton.onClick.AddListener(OnReturnClicked);
    }

    void OnReturnClicked()
    {
        PlayerPrefs.SetString("NextScene", "InitialMap");
        SceneManager.LoadScene("Loading");
    }
}
