using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject optionsPanel;
    public GameObject audioPanel;
    public GameObject controlPanel;
    public GameObject creditPanel;
    public GameObject tutorialPanel;
    public GameObject exitPanel;
    public GameObject dimBackground;
    public GameObject logoInGame;

    [Header("Main Buttons")]
    public GameObject playButtonGO;
    public GameObject optionsButtonGO;
    public GameObject exitButtonGO;

    [Header("Buttons")]
    public Button playButton;
    public Button optionsButton;
    public Button exitButton;

    public Button audioButton;
    public Button controlButton;
    public Button creditButton;
    public Button tutorialButton;
    public Button goBackButton;

    public Button sureButton;
    public Button backButton;

    private bool isButtonLocked = false;

    void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        optionsButton.onClick.AddListener(OnOptionsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        goBackButton.onClick.AddListener(OnGoBackClicked);
        backButton.onClick.AddListener(OnGoBackClicked);
        sureButton.onClick.AddListener(OnSureExitClicked);
        audioButton.onClick.AddListener(OnAudioClicked);
        controlButton.onClick.AddListener(OnControlClicked);
        creditButton.onClick.AddListener(OnCreditClicked);
        tutorialButton.onClick.AddListener(OnTutorialClicked);

        optionsPanel.SetActive(false);
        audioPanel.SetActive(false);
        controlPanel.SetActive(false);
        creditPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        exitPanel.SetActive(false);
        dimBackground.SetActive(false);
        logoInGame.SetActive(true);
        playButtonGO.SetActive(true);
        optionsButtonGO.SetActive(true);
        exitButtonGO.SetActive(true);
    }

    void OnPlayClicked()
    {
        if (isButtonLocked) return;
        isButtonLocked = true;

        // Set scene name cần load sau này (gửi qua PlayerPrefs)
        PlayerPrefs.SetString("NextScene", "MapSelecting");

        // Load scene Loading
        SceneManager.LoadScene("Loading");
    }


    void OnOptionsClicked()
    {
        if (isButtonLocked) return;
        isButtonLocked = true;

        optionsPanel.SetActive(true);
        dimBackground.SetActive(true);
        logoInGame.SetActive(false);
        playButtonGO.SetActive(false);
        optionsButtonGO.SetActive(false);
        exitButtonGO.SetActive(false);

        isButtonLocked = false;
    }

    void OnExitClicked()
    {
        if (isButtonLocked) return;
        isButtonLocked = true;

        exitPanel.SetActive(true);
        dimBackground.SetActive(true);
        logoInGame.SetActive(false);
        playButtonGO.SetActive(false);
        optionsButtonGO.SetActive(false);
        exitButtonGO.SetActive(false);

        isButtonLocked = false;
    }

    void OnAudioClicked()
    {
        if (isButtonLocked) return;
        isButtonLocked = true;

        audioPanel.SetActive(true);
        optionsPanel.SetActive(false);

        isButtonLocked = false;
    }

    void OnControlClicked()
    {
        if (isButtonLocked) return;
        isButtonLocked = true;

        controlPanel.SetActive(true);
        optionsPanel.SetActive(false);

        isButtonLocked = false;
    }

    void OnCreditClicked()
    {
        if (isButtonLocked) return;
        isButtonLocked = true;

        creditPanel.SetActive(true);
        optionsPanel.SetActive(false);

        isButtonLocked = false;
    }

    void OnTutorialClicked()
    {
        if (isButtonLocked) return;
        isButtonLocked = true;

        tutorialPanel.SetActive(true);
        optionsPanel.SetActive(false);

        isButtonLocked = false;
    }

    void OnGoBackClicked()
    {
        if (isButtonLocked) return;
        isButtonLocked = true;

        optionsPanel.SetActive(false);
        audioPanel.SetActive(false);
        controlPanel.SetActive(false);
        creditPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        exitPanel.SetActive(false);
        dimBackground.SetActive(false);
        logoInGame.SetActive(true);
        playButtonGO.SetActive(true);
        optionsButtonGO.SetActive(true);
        exitButtonGO.SetActive(true);

        isButtonLocked = false;
    }

    void OnSureExitClicked()
    {
        if (isButtonLocked) return;
        isButtonLocked = true;

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
 