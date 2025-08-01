using UnityEngine;
using UnityEngine.UI;

public class CreditPanelController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject creditPanel;
    public GameObject optionsPanel;

    [Header("Button")]
    public Button returnButton;

    void Start()
    {
        if (returnButton != null)
            returnButton.onClick.AddListener(OnReturnClicked);
    }

    void OnReturnClicked()
    {

        if (creditPanel != null) creditPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }
}
