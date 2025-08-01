using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SubPanelController : MonoBehaviour
{
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

    private List<GameObject> subPanels;

    void Start()
    {
        subPanels = new List<GameObject> { audioPanel, buttonPanel, creditPanel, tutorialPanel };

        audioBtn?.onClick.AddListener(() => SwitchSubPanel(audioPanel));
        buttonBtn?.onClick.AddListener(() => SwitchSubPanel(buttonPanel));
        creditBtn?.onClick.AddListener(() => SwitchSubPanel(creditPanel));
        tutorialBtn?.onClick.AddListener(() => SwitchSubPanel(tutorialPanel));

        foreach (var panel in subPanels)
            panel?.SetActive(false);
    }

    void SwitchSubPanel(GameObject target)
    {
        foreach (var panel in subPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        if (target != null)
            target.SetActive(true);
    }
}
