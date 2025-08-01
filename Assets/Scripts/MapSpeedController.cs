using UnityEngine;

public class MapSpeedController : MonoBehaviour
{
    public static MapSpeedController Instance;

    [Header("Slow effect")]
    public float slowDuration = 2f;
    public float slowPercent = 0.1f;  // Mỗi lần va chạm trừ 10%
    public float minMultiplier = 0.6f;  // THÊM: Biến hóa min để dễ chỉnh (giới hạn chậm max 40%)

    public float SpeedMultiplier { get; private set; } = 1f;

    private float slowTimer = 0f;
    private float currentSlowAmount = 0f;

    [Header("Boost effect")]
    public float boostDuration = 5f;
    private float boostTimer = 0f;
    private float currentBoostAmount = 0f;
    private bool isGameOver = false;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetGameOver()
    {
        isGameOver = true;
        SpeedMultiplier = 0f;  // THÊM: Set multiplier = 0 để dừng cuộn ngay
    }
    void Update()
    {
        if (isGameOver) return;
        if (slowTimer > 0)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0)
            {
                ResetSlow();
            }
        }
        if (boostTimer > 0)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0)
            {
                ResetBoost();
            }
        }
    }

    public void AddSlowEffect()
    {
        float actualSubtract = slowPercent;
        if (SpeedMultiplier - actualSubtract < minMultiplier)  // SỬA: Dùng biến minMultiplier
        {
            actualSubtract = SpeedMultiplier - minMultiplier;
        }
        if (actualSubtract > 0f)  // THÊM: Chỉ trừ nếu còn chỗ (tránh trừ 0 vô ích)
        {
            SpeedMultiplier -= actualSubtract;
            currentSlowAmount += actualSubtract;
            Debug.Log($"[MapSpeed] Áp dụng slow: Multiplier = {SpeedMultiplier}, total slow amount = {currentSlowAmount}");
        }

        // SỬA: Không reset timer mỗi lần! Chỉ set nếu chưa có slow, hoặc extend một chút (ví dụ +1s) để tránh kéo dài vô hạn
        if (slowTimer <= 0f)
        {
            slowTimer = slowDuration;  // Chỉ set đầy nếu hết
        }
        else
        {
            slowTimer = Mathf.Min(slowTimer + 1f, slowDuration * 2f);  // THÊM: Extend thêm 1s, nhưng max 4s (2x duration) để giới hạn kéo dài
        }
        Debug.Log($"[MapSpeed] Slow timer updated to: {slowTimer}s");
    }

    private void ResetSlow()
    {
        SpeedMultiplier += currentSlowAmount;
        currentSlowAmount = 0f;
        Debug.Log("[MapSpeed] Kết thúc slow: Multiplier = " + SpeedMultiplier);
    }

    public void ApplySpeedBoost(float boostAmount)
    {
        currentBoostAmount += boostAmount;
        SpeedMultiplier += boostAmount;
        boostTimer = boostDuration;  // Có thể giữ reset timer cho boost, vì boost không cộng dồn timer
        Debug.Log($"[MapSpeed] Áp dụng boost: Multiplier = {SpeedMultiplier}, thời gian {boostDuration}s");
    }


    private void ResetBoost()
    {
        SpeedMultiplier -= currentBoostAmount;
        currentBoostAmount = 0f;
        Debug.Log("[MapSpeed] Kết thúc boost: Multiplier = " + SpeedMultiplier);
    }
   
    public float GetCurrentSlowDuration()
    {
        return Mathf.Max(slowTimer, 0f);
    }

    public float GetCurrentBoostDuration()
    {
        return Mathf.Max(boostTimer, 0f);
    }

    public void StartSpeed(float speed = 1f)
    {
        if (isGameOver) return;
        SpeedMultiplier = speed;
        Debug.Log("[MapSpeed] StartSpeed() được gọi. SpeedMultiplier = " + speed);
    }

    public void StopMap()
    {
        SpeedMultiplier = 0f;
        Debug.Log("[MapSpeed] StopMap() được gọi. Map dừng lại.");
    }
    public void SetSpeedMultiplier(float value)
    {
        SpeedMultiplier = value;
        Debug.Log("[MapSpeed] Set multiplier = " + value);
    }


}
