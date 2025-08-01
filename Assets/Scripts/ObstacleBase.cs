using UnityEngine;
using System.Collections;

public class ObstacleBase : MonoBehaviour
{
    public enum ObstacleType { Normal, Explosive }
    public ObstacleType obstacleType = ObstacleType.Normal;
    public bool isAirObstacle = false;
    public bool forceGroundYSpawn = false;
    public GameObject explosionEffectPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D được gọi với collider: " + other.name);
        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        if (player == null) return;
        Debug.Log("Va chạm với Player, loại vật cản: " + obstacleType);

        if (obstacleType == ObstacleType.Normal)
            player.OnObstacleHit();
        else if (obstacleType == ObstacleType.Explosive)
            player.OnObstacleCrash();
        if (obstacleType == ObstacleType.Explosive && explosionEffectPrefab != null)
        {
            // Sử dụng tâm collider để chính xác hơn
            Vector3 effectPos = GetComponent<Collider2D>().bounds.center;
            effectPos.z = 0; // đảm bảo hiệu ứng hiển thị đúng trên mặt 2D
            Instantiate(explosionEffectPrefab, effectPos, Quaternion.identity);
        }
        if (CameraShake.Instance != null)
        {
            if (obstacleType == ObstacleType.Normal)
                CameraShake.Instance.StartShake(0.1f, 0.05f);
            else if (obstacleType == ObstacleType.Explosive)
                CameraShake.Instance.StartShake(0.2f, 0.2f);
        }

        player.StartBlink(3, 0.1f); // Blink player 3 lần
        StartCoroutine(BlinkAndDestroy(3, 0.1f));
    }

    private IEnumerator BlinkAndDestroy(int times, float duration)
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length == 0) yield break;

        // Blink
        for (int i = 0; i < times; i++)
        {
            foreach (var renderer in renderers)
                renderer.enabled = false;

            yield return new WaitForSeconds(duration);

            foreach (var renderer in renderers)
                renderer.enabled = true;

            yield return new WaitForSeconds(duration);
        }

        

        Destroy(gameObject);
    }
}
