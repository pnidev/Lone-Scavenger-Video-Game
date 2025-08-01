using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [Header("Sliders")]
    public Slider musicSlider;
    public Slider effectSlider;
    public Slider dialogSlider;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("UI")]
    public GameObject audioPanel;
    public GameObject optionsPanel;
    public Button returnButton;
    public Button resetButton; // ✅ Nút Reset mới

    private void Start()
    {
        InitSlider(musicSlider, "MusicVolume");
        InitSlider(effectSlider, "SFXVolume");
        InitSlider(dialogSlider, "DialogVolume");

        if (returnButton != null)
            returnButton.onClick.AddListener(OnReturnClicked);

        if (resetButton != null)
            resetButton.onClick.AddListener(ResetVolumes); // ✅ Gán sự kiện Reset
    }

    private void InitSlider(Slider slider, string parameterName)
    {
        float savedValue = PlayerPrefs.GetFloat(parameterName, 0.5f);
        slider.value = savedValue;
        SetVolume(savedValue, parameterName);

        slider.onValueChanged.AddListener((value) =>
        {
            SetVolume(value, parameterName);
        });
    }

    private void SetVolume(float value, string parameterName)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat(parameterName, dB);
        PlayerPrefs.SetFloat(parameterName, value);
    }

    public void ResetVolumes()
    {
        SetAndSave(musicSlider, "MusicVolume", 0.5f);
        SetAndSave(effectSlider, "SFXVolume", 0.5f);
        SetAndSave(dialogSlider, "DialogVolume", 0.5f);
    }

    private void SetAndSave(Slider slider, string parameterName, float value)
    {
        slider.value = value;
        SetVolume(value, parameterName);
    }

    private void OnReturnClicked()
    {
        audioPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
}
