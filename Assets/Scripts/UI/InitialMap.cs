using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OverlayPanelController : MonoBehaviour
{
    [Header("Main Panels")]
    public GameObject pausePanel;
    public GameObject optionPanel;

    [Header("Temporary Panels to Hide")]
    public List<GameObject> temporaryPanelsToHide;

    [Header("Buttons")]
    public Button pauseButton;
    public Button settingButton;
    public Button continueButton;
    public Button goBackButton;
    public Button skipButton;

    [Header("Startup")]
    public GameObject runObject;         // Object cần tắt sau 2 giây
    public GameObject chattingPanel;     // ChattingPanel cần bật sau 2 giây
    public GameObject tutorialPanel;     // Gán từ Unity: panel hướng dẫn

    private List<GameObject> allPanels;
    private GameObject currentPanel;
    private List<GameObject> previouslyActivePanels = new List<GameObject>();

    void Start()
    {
        allPanels = new List<GameObject> { pausePanel, optionPanel };

        // Ẩn tất cả panel chính khi bắt đầu
        foreach (var panel in allPanels)
            panel?.SetActive(false);

        // Gán các nút
        pauseButton?.onClick.AddListener(OpenPausePanel);
        settingButton?.onClick.AddListener(OpenOptionPanel);
        continueButton?.onClick.AddListener(CloseAllPanelsAndResume);
        goBackButton?.onClick.AddListener(CloseOptionPanelAndResume);
        skipButton?.onClick.AddListener(OnSkipTutorial);

        skipButton?.gameObject.SetActive(false); // Ẩn skip ban đầu

        // Tự động chạy sau 2s
        StartCoroutine(ShowChattingAfterDelay(2f));
    }

    IEnumerator ShowChattingAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        if (runObject != null)
            runObject.SetActive(false);

        if (chattingPanel != null)
            chattingPanel.SetActive(true);
    }

    void Update()
    {
        // Nếu tutorialPanel đang bật, thì bật nút skip
        if (tutorialPanel != null && skipButton != null)
        {
            bool isTutorialActive = tutorialPanel.activeSelf;
            skipButton.gameObject.SetActive(isTutorialActive);
        }
    }

    void OpenPausePanel()
    {
        CacheAndHideTemporaryPanels();
        SwitchPanel(pausePanel);
        Time.timeScale = 0;
    }

    void OpenOptionPanel()
    {
        CacheAndHideTemporaryPanels();
        SwitchPanel(optionPanel);
        Time.timeScale = 0;
    }

    void CloseAllPanelsAndResume()
    {
        ClosePanel(pausePanel);
        RestoreTemporaryPanels();
        Time.timeScale = 1;
    }

    void CloseOptionPanelAndResume()
    {
        ClosePanel(optionPanel);
        RestoreTemporaryPanels();
        Time.timeScale = 1;
    }

    void SwitchPanel(GameObject target)
    {
        foreach (var panel in allPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        if (target != null)
        {
            target.SetActive(true);
            currentPanel = target;
        }
    }

    void ClosePanel(GameObject panel)
    {
        if (panel != null && panel.activeSelf)
            panel.SetActive(false);

        if (panel == currentPanel)
            currentPanel = null;
    }

    void CacheAndHideTemporaryPanels()
    {
        previouslyActivePanels.Clear();

        foreach (var panel in temporaryPanelsToHide)
        {
            if (panel != null && panel.activeSelf)
            {
                panel.SetActive(false);
                previouslyActivePanels.Add(panel);
            }
        }
    }

    void RestoreTemporaryPanels()
    {
        foreach (var panel in previouslyActivePanels)
        {
            if (panel != null)
                panel.SetActive(true);
        }

        previouslyActivePanels.Clear();
    }

    void OnSkipTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (skipButton != null)
            skipButton.gameObject.SetActive(false);

        Time.timeScale = 1; // Resume game if it was paused
    }
}
