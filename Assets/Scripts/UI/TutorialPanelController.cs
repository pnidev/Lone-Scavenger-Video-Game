using UnityEngine;
using UnityEngine.UI;

public class TutorialPanelController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject tutorialPanel;
    public GameObject optionsPanel;

    [Header("Return Button")]
    public Button returnButton;

    void Start()
    {
        if (returnButton != null)
            returnButton.onClick.AddListener(OnReturnClicked);
    }

    void OnReturnClicked()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }
}
