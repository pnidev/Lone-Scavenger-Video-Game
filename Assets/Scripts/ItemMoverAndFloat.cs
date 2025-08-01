using UnityEngine;

public class ItemMoverAndFloat : MonoBehaviour
{
    // Phần Float
    //public float floatSpeed = 2f;    // Tốc độ dao động
    //public float floatHeight = 0.1f; // Biên độ dao động

    // Phần Mover
    public float defaultSpeed = 7f;  // Tốc độ di chuyển left (chỉnh thấp để dễ nhặt)

    private Vector3 startPos;
    private float yOffset = 0f;      // Lưu offset Y để relative

    public float floatSpeed = 2f;       // Tốc độ dao động tổng thể
    public float floatHeight = 0.2f;    // Biên độ dao động (tăng nhẹ để tự nhiên, chỉnh 0.1f-0.3f)
    public float noiseScale = 1f;       // Scale cho Perlin Noise (lớn hơn = dao động random nhanh hơn)
    public float rotationAmplitude = 5f; // Biên độ quay nhẹ (độ, ví dụ 5-10 cho xoay subtle)
    private float randomSeed;

    void Start()
    {
        startPos = transform.position;
        randomSeed = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (MapSpeedController.Instance != null && MapSpeedController.Instance.SpeedMultiplier == 0f) return;
        // Di chuyển left (giữ nguyên)
        float multiplier = MapSpeedController.Instance != null ? MapSpeedController.Instance.SpeedMultiplier : 1f;
        float speed = defaultSpeed * multiplier;
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Dao động Y tự nhiên với Perlin Noise (thay vì Sin đơn giản)
        float noise = Mathf.PerlinNoise(Time.time * floatSpeed + randomSeed, 0f) * 2f - 1f;  // Noise từ -1 đến 1
        yOffset = noise * floatHeight;  // Áp dụng noise vào height

        transform.position = new Vector3(transform.position.x, startPos.y + yOffset, transform.position.z);

        // Thêm rotation nhẹ tự nhiên (sin + noise nhỏ để random)
        float rotationNoise = Mathf.PerlinNoise(Time.time * floatSpeed * 0.5f + randomSeed + 50f, 0f) * 2f - 1f;  // Noise khác cho rotation
        float rotationAngle = Mathf.Sin(Time.time * floatSpeed) * rotationAmplitude + rotationNoise * 2f;  // Kết hợp sin và noise
        transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);

        // Destroy khi ra khỏi màn
        if (transform.position.x < -30f)
        {
            Destroy(gameObject);
        }
    }
}