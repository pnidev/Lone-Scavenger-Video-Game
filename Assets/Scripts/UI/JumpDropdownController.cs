using UnityEngine;
using UnityEngine.UI;

public class JumpDropdownController : MonoBehaviour
{
    [Header("Dropdown Panels")]
    public GameObject jumpDropdown1; // Chứa nút Jump(W) mặc định
    public GameObject jumpDropdown2; // Chứa nút Jump(↑)

    [Header("Main Buttons")]
    public GameObject jumpWButtonMain; // Nút Jump(W) chính mặc định
    public GameObject jumpUpButton;    // Nút chọn "↑"

    private string currentKey = "W"; // Giá trị hiện tại

    void Start()
    {
        // Gán sự kiện click
        jumpWButtonMain.GetComponent<Button>().onClick.AddListener(OnJumpWClick);
        jumpDropdown1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(OnJumpDropdown1Click);
        jumpUpButton.GetComponent<Button>().onClick.AddListener(OnJumpUpClick);
        jumpDropdown2.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(OnJumpDropdown2Click);

        ResetToDefault();
    }

    public void OnJumpWClick()
    {
        jumpWButtonMain.SetActive(false);
        jumpDropdown1.SetActive(true);
    }

    public void OnJumpDropdown1Click()
    {
        jumpDropdown1.SetActive(false);
        jumpUpButton.SetActive(true);
        jumpDropdown2.SetActive(true);
        currentKey = "↑";
    }

    public void OnJumpUpClick()
    {
        jumpUpButton.SetActive(false);
        jumpWButtonMain.SetActive(true);
        jumpDropdown1.SetActive(true);
        jumpDropdown2.SetActive(false);
        currentKey = "W";
    }

    public void OnJumpDropdown2Click()
    {
        jumpDropdown2.SetActive(false);
        jumpWButtonMain.SetActive(true);
        currentKey = "↑";
    }

    public void ResetToDefault()
    {
        currentKey = "W";

        jumpWButtonMain.SetActive(true);
        jumpUpButton.SetActive(false);

        jumpDropdown1.SetActive(false);
        jumpDropdown2.SetActive(false);
    }

    public string GetCurrentKey()
    {
        return currentKey;
    }
}
