using UnityEngine;
using UnityEngine.UI;

public class TutorialPageSwitcher : MonoBehaviour
{
    public GameObject currentPage;
    public GameObject targetPage;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SwitchPage);
    }

    void SwitchPage()
    {
        if (currentPage != null) currentPage.SetActive(false);
        if (targetPage != null) targetPage.SetActive(true);
    }
}
