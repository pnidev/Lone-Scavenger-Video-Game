using UnityEngine;
using UnityEngine.UI;  // Cho Button/Image
using TMPro;          // Cho TextMeshPro
using System;         // Cho TimeSpan timer

public class TopBarManager : MonoBehaviour
{
    public static TopBarManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI timerText;  // Kéo TimerText vào Inspector
    public Button soundButton;         // Kéo SoundButton vào
    public Image soundIcon;            // Kéo Image con của SoundButton (icon loa)

    [Header("Icons for Sound")]
    public Sprite soundOnIcon;         // Icon loa on (từ bộ icon sẵn)
    public Sprite soundOffIcon;        // Icon loa off (từ bộ icon sẵn)

    private bool isMuted = false;      // Trạng thái mute sound

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Gán sự kiện cho button
        if (soundButton != null) soundButton.onClick.AddListener(ToggleSound);

        UpdateSoundIcon();  // Cập nhật icon sound ban đầu
    }

    void Update()
    {
        UpdateTimerUI();  // Đồng bộ timer từ Global
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            TimeSpan time = TimeSpan.FromSeconds(GlobalDataManager.Instance.elapsedTime);
            timerText.text = time.ToString("mm':'ss':'ff");  // Format 00:01:03
        }
    }

    private void ToggleSound()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;  // Mute/unmute toàn bộ sound
        UpdateSoundIcon();
        Debug.Log("[TopBar] Sound muted: " + isMuted);
    }

    private void UpdateSoundIcon()
    {
        if (soundIcon != null)
        {
            soundIcon.sprite = isMuted ? soundOffIcon : soundOnIcon;  // Chuyển icon khi click
        }
    }
}