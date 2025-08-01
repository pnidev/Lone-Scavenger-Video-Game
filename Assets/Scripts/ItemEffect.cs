using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public enum ItemType { Boost, LevelUp }
    public ItemType itemType; // Chọn Boost hoặc LevelUp trong Inspector prefab

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        if (player == null) return;

        if (itemType == ItemType.Boost)
        {
            // Gọi method công khai để boost (tăng 0.5f = 50%)
            if (MapSpeedController.Instance != null)
            {
                MapSpeedController.Instance.ApplySpeedBoost(0.5f);  // Chỉnh amount nếu cần (ví dụ 0.3f cho tăng 30%)
                player.OnSpeedBoost(MapSpeedController.Instance.boostDuration);
                Animator anim = other.GetComponent<Animator>();
                //if (anim != null)
                //{
                //    anim.SetTrigger("BoostTrigger"); // Gắn đúng với Animator
                //}
            }

        }
        else if (itemType == ItemType.LevelUp)
            if (WeaponHUD.Instance != null)
            {
                WeaponHUD.Instance.AddEnergyBar();
                Debug.Log("[ItemEffect] Gọi thành công WeaponHUD.AddEnergyBar()");  // THÊM: Trace gọi HUD
            }
            else
            {
                Debug.LogError("[ItemEffect] WeaponHUD.Instance là null! Không gọi được AddEnergyBar().");  // THÊM: Trace lỗi null
            }

        Destroy(gameObject); // Xóa item sau khi nhặt
    }
}