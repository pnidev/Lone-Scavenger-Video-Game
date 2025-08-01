using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ControlSetting : MonoBehaviour
{
    [Header("Key Bindings Display")]
    public Text jumpKeyText;
    public Text crouchKeyText;
    public Text shootKeyText;

    [Header("Dropdown Panels")]
    public GameObject jumpDropdown;
    public GameObject crouchDropdown;
    public GameObject shootDropdown;

    [Header("Arrows")]
    public GameObject jumpArrowDown;
    public GameObject jumpArrowUp;
    public GameObject crouchArrowDown;
    public GameObject crouchArrowUp;
    public GameObject shootArrowDown;
    public GameObject shootArrowUp;

    [Header("Buttons")]
    public Button resetButton;
    public Button returnButton;

    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    private string waitingForKey = null;

    void Start()
    {
        LoadDefaultKeysIfNeeded();
        UpdateUI();

        jumpKeyText.GetComponentInParent<Button>().onClick.AddListener(() => StartRebinding("Jump"));
        crouchKeyText.GetComponentInParent<Button>().onClick.AddListener(() => StartRebinding("Crouch"));
        shootKeyText.GetComponentInParent<Button>().onClick.AddListener(() => StartRebinding("Shoot"));

        resetButton.onClick.AddListener(ResetToDefault);
        returnButton.onClick.AddListener(SaveAndReturn);

        jumpArrowDown.GetComponent<Button>().onClick.AddListener(() => ToggleDropdown("Jump", true));
        jumpArrowUp.GetComponent<Button>().onClick.AddListener(() => ToggleDropdown("Jump", false));

        // Thêm tương tự cho Crouch và Shoot nếu cần
    }

    void Update()
    {
        if (waitingForKey != null && Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    keyBindings[waitingForKey] = keyCode;
                    waitingForKey = null;
                    UpdateUI();
                    ToggleDropdown("Jump", false); // Ẩn dropdown sau khi chọn xong
                    break;
                }
            }
        }
    }

    void StartRebinding(string action)
    {
        waitingForKey = action;
    }

    void UpdateUI()
    {
        jumpKeyText.text = keyBindings["Jump"].ToString();
        crouchKeyText.text = keyBindings["Crouch"].ToString();
        shootKeyText.text = keyBindings["Shoot"].ToString();
    }

    void ToggleDropdown(string action, bool show)
    {
        switch (action)
        {
            case "Jump":
                jumpDropdown.SetActive(show);
                jumpArrowDown.SetActive(!show);
                jumpArrowUp.SetActive(show);
                break;
            case "Crouch":
                crouchDropdown.SetActive(show);
                crouchArrowDown.SetActive(!show);
                crouchArrowUp.SetActive(show);
                break;
            case "Shoot":
                shootDropdown.SetActive(show);
                shootArrowDown.SetActive(!show);
                shootArrowUp.SetActive(show);
                break;
        }
    }

    void LoadDefaultKeysIfNeeded()
    {
        keyBindings["Jump"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("JumpKey", "W"));
        keyBindings["Crouch"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("CrouchKey", "S"));
        keyBindings["Shoot"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ShootKey", "Space"));
    }

    void ResetToDefault()
    {
        keyBindings["Jump"] = KeyCode.W;
        keyBindings["Crouch"] = KeyCode.S;
        keyBindings["Shoot"] = KeyCode.Space;
        UpdateUI();
    }

    void SaveAndReturn()
    {
        PlayerPrefs.SetString("JumpKey", keyBindings["Jump"].ToString());
        PlayerPrefs.SetString("CrouchKey", keyBindings["Crouch"].ToString());
        PlayerPrefs.SetString("ShootKey", keyBindings["Shoot"].ToString());

        PlayerPrefs.Save();
        gameObject.SetActive(false);
    }
}
