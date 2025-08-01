using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelSwitcher : MonoBehaviour
{
    [Header("Main Panel")]
    public GameObject optionsPanel; // Panel chính chứa các nút chọn (audioBtn, buttonBtn, v.v.)

    [Header("Sub Panels")]
    public GameObject audioPanel;
    public GameObject buttonPanel;
    public GameObject creditPanel;
    public GameObject tutorialPanel;

    [Header("Buttons")]
    public Button audioBtn;
    public Button buttonBtn;
    public Button creditBtn;
    public Button tutorialBtn;
    public Button goBackBtn; // Nút back để quay về optionsPanel

    private List<GameObject> allPanels;

    void Awake()
    {
        allPanels = new List<GameObject> { optionsPanel, audioPanel, buttonPanel, creditPanel, tutorialPanel }; // Thêm optionsPanel vào list để quản lý chung

        if (audioBtn != null) audioBtn.onClick.AddListener(() => SwitchToPanel(audioPanel));
        if (buttonBtn != null) buttonBtn.onClick.AddListener(() => SwitchToPanel(buttonPanel));
        if (creditBtn != null) creditBtn.onClick.AddListener(() => SwitchToPanel(creditPanel));
        if (tutorialBtn != null) tutorialBtn.onClick.AddListener(() => SwitchToPanel(tutorialPanel));

        if (goBackBtn != null) goBackBtn.onClick.AddListener(() => SwitchToPanel(optionsPanel)); // Gán event cho nút back: Quay về optionsPanel

        foreach (var panel in allPanels)
            if (panel != null) panel.SetActive(false); // Tắt tất cả panel lúc đầu

        if (optionsPanel != null) optionsPanel.SetActive(true); // Bật optionsPanel làm panel mặc định
    }

    void SwitchToPanel(GameObject target)
    {
        foreach (var panel in allPanels)
            if (panel != null) panel.SetActive(false); // Tắt tất cả panel

        if (target != null)
        {
            target.SetActive(true); // Bật panel được chọn
            Debug.Log("Bật panel: " + target.name); // Log để debug
        }
    }
}