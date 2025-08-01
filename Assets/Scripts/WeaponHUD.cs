using UnityEngine;
using UnityEngine.UI;  // Cho Image
using TMPro;          // Cho TextMeshPro nếu dùng

public class WeaponHUD : MonoBehaviour
{
    public static WeaponHUD Instance;  // Singleton để gọi từ Player/Item

    [Header("Hearts (Blood)")]
    public Image[] heartImages;  // Kéo 3 Heart Image vào Inspector (từ Heart1 đến Heart3)
    public Sprite heartFull;     // Sprite trái tim sáng (cam nhạt)

    [Header("Level")]
    public TextMeshProUGUI levelText;  // Kéo LevelText vào

    [Header("Energy Bars")]
    public Image[] energyBarImages;  // Kéo 5 EnergyBar Image vào (EnergyBar1 đến EnergyBar5)

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateHeartsUI();
        UpdateLevelUI();
        ResetEnergyBarsUI();  // Ban đầu tất cả ẩn
        UpdateEnergyBarsUI();  // Đồng bộ từ Global
    }

    // Hàm mất mạng (gọi từ Player khi lose life)
    public void LoseHeart()
    {
        GlobalDataManager.Instance.currentHearts--;
        if (GlobalDataManager.Instance.currentHearts < 0) GlobalDataManager.Instance.currentHearts = 0;
        UpdateHeartsUI();
        if (GlobalDataManager.Instance.currentHearts <= 0)
        {
            Debug.Log("Game Over - Hết máu!");
            FindObjectOfType<Player>()?.Fall();  // Gọi nếu Player tồn tại
        }
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
            {
                heartImages[i].gameObject.SetActive(i < GlobalDataManager.Instance.currentHearts);  // Ẩn nếu i >= currentHearts
                heartImages[i].sprite = heartFull;  // Luôn dùng sprite full cho tim còn lại
                Debug.Log("[WeaponHUD] Update heart " + (i + 1) + ": Active = " + heartImages[i].gameObject.activeSelf);
            }
            else
            {
                Debug.LogError("[WeaponHUD] Heart Image tại index " + i + " là null!");
            }
        }
    }

    // Hàm add energy từ item LevelUp (gọi từ ItemEffect/Player)
    public void AddEnergyBar()
    {
        Debug.Log("[WeaponHUD] AddEnergyBar() được gọi - Trước: currentEnergyBars = " + GlobalDataManager.Instance.currentEnergyBars);
        GlobalDataManager.Instance.currentEnergyBars++;
        if (GlobalDataManager.Instance.currentEnergyBars > 5)
        {
            GlobalDataManager.Instance.currentEnergyBars = 1;  // Reset về 1
            GlobalDataManager.Instance.currentLevel++;        // Tăng level
            UpdateLevelUI();
            ResetEnergyBarsUI();   // Ẩn tất cả
        }
        UpdateEnergyBarsUI();      // Luôn gọi để update
        Debug.Log("[WeaponHUD] Sau: currentEnergyBars = " + GlobalDataManager.Instance.currentEnergyBars + ", Level = " + GlobalDataManager.Instance.currentLevel);
    }

    private void UpdateLevelUI()
    {
        if (levelText != null)
            levelText.text = $"LV:{GlobalDataManager.Instance.currentLevel:00}";  // Format 01, 02,...
    }

    // Cập nhật hiển thị energy bars
    private void UpdateEnergyBarsUI()
    {
        if (energyBarImages == null || energyBarImages.Length == 0)
        {
            Debug.LogError("[WeaponHUD] energyBarImages chưa được gán trong Inspector!");
            return;
        }
        for (int i = 0; i < energyBarImages.Length; i++)
        {
            if (energyBarImages[i] != null)
            {
                energyBarImages[i].gameObject.SetActive(i < GlobalDataManager.Instance.currentEnergyBars);  // Bật nếu index < bars
                Debug.Log("[WeaponHUD] SetActive cho EnergyBar" + (i + 1) + ": " + (i < GlobalDataManager.Instance.currentEnergyBars));
            }
            else
            {
                Debug.LogError("[WeaponHUD] EnergyBar Image tại index " + i + " là null!");
            }
        }
    }

    // Ẩn tất cả bars (gọi ở Start hoặc reset)
    private void ResetEnergyBarsUI()
    {
        for (int i = 0; i < energyBarImages.Length; i++)
        {
            if (energyBarImages[i] != null)
                energyBarImages[i].gameObject.SetActive(false);  // Ẩn tất cả
        }
    }

    public int GetCurrentHearts()
    {
        return GlobalDataManager.Instance.currentHearts;
    }
}