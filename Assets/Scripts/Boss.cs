using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
public class Boss : MonoBehaviour
{
    private Animator animator;
    public int health = 7; // Máu boss
    public GameObject explosionPrefab; // Kéo prefab Explosion (từ PNG anh)
    public GameObject dieExplosionPrefab;                                   // Thêm field đầu script
    public GameObject bulletPrefab; // // Kéo BulletBossPrefab
    public Transform firePoint; // // Kéo FirePoint child

    private float timer; // // Timer đếm
    private float shootTimer;
    public float minShootInterval = 1f;
    public float maxShootInterval = 3f;
    [SerializeField] private SpriteRenderer bossSpriteRenderer;

    public Slider healthSlider;
    private int maxHealth;
    private bool isDead = false;

    public AudioClip shootBulletSound; // 🎵 Âm thanh bắn
    private AudioSource audioSource;




    void Start()
    {
        audioSource = GetComponent<AudioSource>();



        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("[Boss] Animator null!");
        shootTimer = Random.Range(minShootInterval, maxShootInterval);
        if (bossSpriteRenderer == null)
            bossSpriteRenderer = GetComponent<SpriteRenderer>();

        // THÊM: Init thanh HP
        maxHealth = health;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
            healthSlider.gameObject.SetActive(true); // Bật nếu ẩn
            Debug.Log("Thanh HP boss initialized: " + health + "/" + maxHealth);
        }
    }
    void Update()
    {
        // Debug mỗi frame để trace
        if (isDead) return;
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {

            Shoot();
            shootTimer = Random.Range(minShootInterval, maxShootInterval);
        }
    }


    public void TakeDamage(int dmg)
    {
        health -= dmg;
        healthSlider.value = health;
        if (health <= 0)
        {
            isDead = true;
            if (dieExplosionPrefab != null)
                Instantiate(dieExplosionPrefab, transform.position, Quaternion.identity);
            else
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Bắt đầu fade out ngay lập tức (không chờ thêm ở đây)
            StartCoroutine(FadeOutEffect(0.5f));

            MapSpeedController.Instance.StopMap();
            Debug.Log("Boss chết! Win Phase 2");

            // Delay riêng cho panel victory (2 giây)
            StartCoroutine(ShowVictoryAfterDelay(2f));

            // Delay riêng cho destroy boss (2.1 giây, sau panel một chút)
            StartCoroutine(DestroyAfterDelay(2.1f));

            // Xóa đạn ngay lập tức
            BulletBoss[] bullets = FindObjectsOfType<BulletBoss>();
            foreach (BulletBoss bullet in bullets)
            {

                Destroy(bullet.gameObject);
            }

            if (healthSlider != null)
                healthSlider.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Boss trúng đạn! Máu còn: " + health);
            StartBlink(3, 0.1f);
        }
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.StartShake(0.2f, 0.1f);
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    //private IEnumerator ShowVictoryAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //    VictoryUIController victoryUI = FindFirstObjectByType<VictoryUIController>();
    //    if (victoryUI != null)
    //    {
    //        victoryUI.ShowVictoryPanel(3); // Hiển thị với 3 sao (thay đổi nếu cần)
    //    }
    //    else
    //    {
    //        Debug.LogError("VictoryUIController không tìm thấy trong scene!");
    //    }
    //}
    //private void Shoot()
    //{
    //    if (animator == null)
    //    {
    //        Debug.LogError("Animator null in Shoot!");
    //        return;
    //    }
    //    animator.SetTrigger("ShootTrigger");

    //    if (bulletPrefab == null) Debug.LogError("bulletPrefab null!");
    //    if (firePoint == null) Debug.LogError("firePoint null!");
    //    if (bulletPrefab != null && firePoint != null)
    //    {
    //        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    //        Debug.Log("Spawned bullet at: " + bullet.transform.position);  // Debug position
    //    }
    //    else
    //    {
    //        Debug.LogError("Cannot spawn bullet - prefab or firePoint null!");
    //    }
    //}
    private void Shoot()
    {
        if (animator == null)
        {
            Debug.LogError("Animator null in Shoot!");
            return;
        }
        animator.SetTrigger("ShootTrigger");  // Trigger animation shoot ngay


        // THÊM: Start Coroutine để delay 1s trước khi spawn đạn
        StartCoroutine(DelaySpawnBullet(1f));  // Delay 1 giây (thay đổi nếu cần)
       

    }

    private IEnumerator DelaySpawnBullet(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);  // Chờ 1s

        if (bulletPrefab == null) Debug.LogError("bulletPrefab null!");
        if (firePoint == null) Debug.LogError("firePoint null!");
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Debug.Log("Spawned bullet at: " + bullet.transform.position);  // Debug position
                                                                           // 🎵 Phát tiếng bắn
            if (shootBulletSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootBulletSound);
            }

        }
        else
        {
            Debug.LogError("Cannot spawn bullet - prefab or firePoint null!");
        }
    }
    public void StartBlink(int blinkTimes = 3, float singleBlinkDuration = 0.1f)
    {
        StartCoroutine(BlinkCoroutine(blinkTimes, singleBlinkDuration));
    }

    // THÊM: Coroutine Blink (copy từ Player)
    private IEnumerator BlinkCoroutine(int times, float duration)
    {
        if (bossSpriteRenderer == null)
        {
            bossSpriteRenderer = GetComponent<SpriteRenderer>();  // Tự get nếu chưa
            if (bossSpriteRenderer == null) yield break;  // Không có thì skip
        }

        for (int i = 0; i < times; i++)
        {
            bossSpriteRenderer.enabled = false;  // Off
            yield return new WaitForSeconds(duration);
            bossSpriteRenderer.enabled = true;   // On
            yield return new WaitForSeconds(duration);
        }
    }
    private IEnumerator DieEffect()
    {
        // Fade out boss sprite (mờ dần trong 1 giây)
        float fadeDuration = 0.5f;
        float elapsed = 0f;
        Color startColor = bossSpriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            bossSpriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // Delay thêm 1.5s để giống logic cũ (0.5 fade + 1.5 = 2s)
        yield return new WaitForSeconds(1.5f);

        VictoryUIController victoryUI = FindFirstObjectByType<VictoryUIController>();
        if (victoryUI != null)
        {
            int earnedStars = GlobalDataManager.Instance.CalculateStarRating(); // 👈 gọi tính sao thật
            victoryUI.ShowVictoryPanel(earnedStars);                            // 👈 truyền sao đúng
        }
        else
        {
            Debug.LogError("VictoryUIController không tìm thấy trong scene!");
        }
        BulletBoss[] bullets = FindObjectsOfType<BulletBoss>();
        foreach (BulletBoss bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }


        Destroy(gameObject); // 💥 Destroy sau khi hiện panel

    }
    // Coroutine chờ 2 giây rồi hiển thị panel
    private IEnumerator ShowVictoryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        VictoryUIController victoryUI = FindFirstObjectByType<VictoryUIController>();
        if (victoryUI != null)
        {
            int earnedStars = 3; // Giá trị mặc định nếu lỗi
            if (GlobalDataManager.Instance != null)
            {
                try
                {
                    earnedStars = GlobalDataManager.Instance.CalculateStarRating();
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Lỗi ở GlobalDataManager.CalculateStarRating: " + e.Message);
                    earnedStars = 3; // Dùng mặc định nếu lỗi
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy GlobalDataManager, dùng 3 sao mặc định.");
            }
            victoryUI.ShowVictoryPanel(earnedStars);
        }
        else
        {
            Debug.LogError("VictoryUIController không tìm thấy trong scene!");
        }
    }
    private IEnumerator FadeOutEffect(float fadeDuration)
    {
        float elapsed = 0f;
        Color startColor = bossSpriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            bossSpriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
    }
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}