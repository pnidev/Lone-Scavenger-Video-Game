using UnityEngine;
using UnityEngine.UI;

public class KeyRebindSimpleController : MonoBehaviour
{
    [Header("Key Setup")]
    public string defaultKey = "W";
    public string altKey = "↑";

    [Header("Buttons")]
    public GameObject mainButton;
    public GameObject dropdown1;
    public GameObject altButton;
    public GameObject dropdown2;

    [Header("Dropdown Option Buttons")]
    public Button dropdown1OptionButton;
    public Button dropdown2OptionButton;

    private string currentKey;

    void Start()
    {
        currentKey = defaultKey;

        mainButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            dropdown1.SetActive(true);
        });

        dropdown1OptionButton.onClick.AddListener(() =>
        {
            mainButton.SetActive(false);
            dropdown1.SetActive(false);
            altButton.SetActive(true);
            currentKey = altKey;
        });

        altButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            dropdown2.SetActive(true);
        });

        dropdown2OptionButton.onClick.AddListener(() =>
        {
            altButton.SetActive(false);
            dropdown2.SetActive(false);
            mainButton.SetActive(true);
            currentKey = defaultKey;
        });

        ResetToDefault();
    }

    public void ResetToDefault()
    {
        currentKey = defaultKey;
        mainButton.SetActive(true);
        dropdown1.SetActive(false);
        altButton.SetActive(false);
        dropdown2.SetActive(false);
    }

    public string GetCurrentKey()
    {
        return currentKey;
    }
}
