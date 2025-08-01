using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private float jumpForce = 15;
    private Rigidbody2D rb;
    private bool isGrounded;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius = 0.2f;
    [SerializeField]
    private LayerMask groundLayer; //chon layer dat
    private Animator anim;
    [SerializeField]
    private BoxCollider2D runCollider;
    [SerializeField]
    private CapsuleCollider2D crouchCollider;

    [SerializeField] private int maxObstacleHits = 4;
    private int obstacleHitCount = 0;
    [SerializeField] int lifeCount = 3;
    private bool wasGroundedLastFrame = true;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    private Vector3 defaultLocalPos;
    private Coroutine speedEffectCoroutine;
    private Vector3 basePos;                        // Vị trí gốc (global, lưu full nhưng chỉ shift X)
    private float targetXOffset = 0f;               // Offset mục tiêu chỉ X (left/right)
    private float shiftSmoothSpeed = 2f;            // Tốc độ Lerp mượt
    public bool isControllable = true;
    private bool isBeingPushedBack = false;
    [SerializeField] private Sprite idleSprite;

    public GameObject bulletPrefab; // // Kéo BulletPlayerPrefab vào Inspector
    public Transform firePoint; // // Kéo FirePoint child vào
    private bool inPhase2 = false; // // Flag check Phase 2

    public bool isJumping = false;
    //private bool isBoostActive = false;
    public GameObject losePanel;
    [SerializeField] private AudioClip shootSFX;
    private AudioSource audioSource;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Time.timeScale = 1f;
        if (playerSpriteRenderer == null)
            playerSpriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("isCrouching", false);
        runCollider.enabled = true;
        crouchCollider.enabled = false;
        basePos = transform.position;  // Lưu vị trí gốc global

        inPhase2 = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Phase2Scene";
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        inPhase2 = currentScene == "Phase2Scene";
        if (currentScene == "CutScene")
        {
            isControllable = false;
            Debug.Log("Player input disabled in CutScene.");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!isControllable) return;
        isGrounded = CheckIfGrounded();
        HanldeJump();
        HandleCrouch();
        //anim.SetBool("isBoosting", isBoostActive);
        //UpdateShiftPosition();
        if (isBeingPushedBack)
        {
            float currentX = transform.position.x;
            float desiredX = basePos.x + targetXOffset;
            float lerpedX = Mathf.Lerp(currentX, desiredX, Time.deltaTime * shiftSmoothSpeed);
            transform.position = new Vector3(lerpedX, transform.position.y, transform.position.z);

            if (Mathf.Abs(currentX - desiredX) < 0.01f)
            {
                isBeingPushedBack = false;
                basePos.x = transform.position.x;
                targetXOffset = 0f;  // reset offset
                if (anim != null)
                    anim.enabled = false;

                // Set Sprite đứng yên
                if (playerSpriteRenderer != null && idleSprite != null)
                    playerSpriteRenderer.sprite = idleSprite;
                Debug.Log("Push back completed.");
            }
        }
        else
        {
            UpdateShiftPosition();
        }
        if (inPhase2)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // // Lock x=0, giữ y cho nhảy
            //anim.SetBool("isRunning", true); // // Nếu cần bool for Run (nếu default state chưa đủ)

            // New code: Bắn khi nhấn RightShift
            if (Input.GetKeyDown(KeyCode.E))
            {
                Shoot();
            }
        }


    }

    //check coi co dang dung o mat dat ko
    private bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    //ve hinh tron mau do va ban kinh
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    //thuc hien hanh dong nhay
    private void HanldeJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetBool("isJumping", true);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset y để không cộng dồn lực
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = true;

        }

        // Nếu vừa từ không khí chuyển sang chạm đất
        if (isGrounded && !wasGroundedLastFrame)
        {
            anim.SetBool("isJumping", false);
            isJumping = false;
        }

        // Lưu trạng thái frame trước
        wasGroundedLastFrame = isGrounded;
    }
    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("dang nhan xuong");
            runCollider.enabled = false;
            crouchCollider.enabled = true;
            anim.SetBool("isCrouching", true);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            runCollider.enabled = true;
            crouchCollider.enabled = false;
            anim.SetBool("isCrouching", false);
        }
    }

    public void OnObstacleHit()
    {
        obstacleHitCount++;
        Debug.Log("Vấp phải vật cản. Đếm: " + obstacleHitCount);

        // Gọi hiệu ứng slow ở đây nếu có
        if (MapSpeedController.Instance != null)
        {
            MapSpeedController.Instance.AddSlowEffect();
            float duration = MapSpeedController.Instance.GetCurrentSlowDuration();
            ApplySpeedEffect(false, duration);
        }
        else
        {
            Debug.LogError("MapSpeedController.Instance là null! Không thể gọi AddSlowEffect.");
        }

        if (obstacleHitCount >= maxObstacleHits)
        {
            Debug.Log("Quá 4 lần → mất mạng");
            // Gọi hàm mất mạng
            LoseLife();
            obstacleHitCount = 0;
        }
    }
    
    public void OnObstacleCrash()
    {
        Debug.Log("Va vào vật nổ → mất mạng ngay");
        LoseLife();
    }

    public void LoseLife()
    {
        lifeCount--;
        Debug.Log("Còn lại: " + lifeCount);
        if (lifeCount <= 0)
        {
            Debug.Log("Game Over!");
            Fall();
            // Gọi cảnh thua hoặc restart game
        }
        WeaponHUD.Instance?.LoseHeart();
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.StartShake(0.5f, 0.2f); // // Rung mạnh 0.5s
        }
        StartBlink(3, 0.1f);
    }

    // Thêm hàm để nhận thông báo từ HUD khi hết máu
    // Trong Player.cs, sửa OnGameOverFromHUD()
    public void OnGameOverFromHUD()
    {
        if (WeaponHUD.Instance != null && WeaponHUD.Instance.GetCurrentHearts() <= 0) 
        {
            Fall();
        }
    }

    public void StartBlink(int blinkTimes = 3, float singleBlinkDuration = 0.1f)
    {
        StartCoroutine(BlinkCoroutine(blinkTimes, singleBlinkDuration));
    }
    private IEnumerator BlinkCoroutine(int times, float duration)
    {
        if (playerSpriteRenderer == null)
        {
            playerSpriteRenderer = GetComponent<SpriteRenderer>();  // Tự get nếu chưa assign
            if (playerSpriteRenderer == null) yield break;  // Không có renderer thì skip
        }

        for (int i = 0; i < times; i++)
        {
            playerSpriteRenderer.enabled = false;  // Off
            yield return new WaitForSeconds(duration);
            playerSpriteRenderer.enabled = true;   // On
            yield return new WaitForSeconds(duration);
        }
    }

    public void ApplySpeedEffect(bool isBoost, float duration)
    {

        if (speedEffectCoroutine != null)
            StopCoroutine(speedEffectCoroutine);

        speedEffectCoroutine = StartCoroutine(SpeedShift(isBoost, duration));
    }
    private IEnumerator SpeedShift(bool isBoost, float duration)
    {
        targetXOffset = isBoost ? 2f : -1.5f;  // + cho boost (right), - cho slow (left)
        yield return new WaitForSeconds(duration);
        targetXOffset = 0f;  // Hết thời gian, về 0
    }

    private void UpdateShiftPosition()
    {
        float desiredX = basePos.x + targetXOffset;     // Chỉ tính X đích
        float currentX = transform.position.x;
        float lerpedX = Mathf.Lerp(currentX, desiredX, Time.deltaTime * shiftSmoothSpeed);  // Lerp mượt chỉ X

        // Set position: Chỉ thay X, giữ Y/Z nguyên (không ảnh hưởng jump)
        transform.position = new Vector3(lerpedX, transform.position.y, transform.position.z);
        if (isBeingPushedBack && Mathf.Abs(currentX - desiredX) < 0.01f)
        {
            isBeingPushedBack = false;
            Debug.Log("Push back completed.");
        }
    }


    public void OnSpeedBoost(float duration)
    {
       
        
        ApplySpeedEffect(true, duration);
        
    }
    // THÊM: Hàm Fall() để xử lý thua game
    public void Fall()
    {


        if (anim != null)
        {
            anim.SetTrigger("FallTrigger");  // SỬA: SetTrigger cho Trigger mới
            // Reset các Bool khác để tránh conflict
            anim.SetBool("isJumping", false);
            anim.SetBool("isCrouching", false);
            Debug.Log("SetTrigger FallTrigger thành công, reset Jump/Crouch");
        }
        else
        {
            Debug.LogError("Animator is null - không trigger được!");
        }
        // Dừng vật lý và input
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;  // Không rơi, không nhảy nữa

        if (inPhase2)
        {
            // Dừng map cuộn
            if (MapSpeedController.Instance != null)
            {
                MapSpeedController.Instance.StopMap();  // Dừng map scrolling
                Debug.Log("Map stopped when player dies in Phase 2.");
            }

            // Tắt anim run của boss
            Boss boss = FindFirstObjectByType<Boss>();  // Tìm Boss trong scene
            if (boss != null)
            {
                Animator bossAnimator = boss.GetComponent<Animator>();
                if (bossAnimator != null)
                {
                    bossAnimator.enabled = false;  // Tắt Animator để stop anim run
                    // Hoặc nếu boss có bool "isRunning": bossAnimator.SetBool("isRunning", false);
                    Debug.Log("Boss run animation disabled when player dies in Phase 2.");
                }
                else
                {
                    Debug.LogWarning("Boss Animator not found in Phase 2!");
                }
            }
            else
            {
                Debug.LogWarning("Boss not found in Phase 2!");
            }
            boss.enabled = false;
        }
        // Dừng các hệ thống khác (map, spawn)
        MapSpeedController.Instance?.SetGameOver();  // Gọi nếu có Instance
        FindFirstObjectByType<ObstacleSpawner>()?.SetGameOver();
        FindFirstObjectByType<ItemSpawner>()?.SetGameOver();
        FindFirstObjectByType<ParallaxLooper>()?.SetGameOver();  // Nếu có nhiều layer, gọi cho từng
        StartCoroutine(ShowLosePanelAfterDelay(3f));
       

       
    }

    // THÊM: Coroutine chờ 3s rồi hiện panel
    private IEnumerator ShowLosePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (losePanel != null)
        {
            losePanel.SetActive(true);
            Time.timeScale = 0f;  // Dừng game sau khi hiện panel
            Debug.Log("Panel Lose hiện sau 3s, game dừng!");
        }
        else
        {
            Debug.LogError("LosePanel chưa assign!");
        }

        // Dừng timer nếu có
        if (GlobalDataManager.Instance != null)
        {
            GlobalDataManager.Instance.isTimerRunning = false;
        }
    }
    public void StartPushBack()
    {
        isBeingPushedBack = true;
        targetXOffset = -2.5f;    // Lùi về trái (hoặc số khác tùy ý)
        shiftSmoothSpeed = 3f;    // Tốc độ lùi
        Debug.Log("Player bị lùi về sau từ Timeline.");
    }
    private void Shoot()
    {
        if (anim != null) anim.SetTrigger("ShootTrigger");
        if (bulletPrefab != null && firePoint != null)
        {
            //Vector3 spawnPos = firePoint.position + firePoint.right * 1f;
            Vector3 spawnPos = firePoint.position;
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, firePoint.rotation);
            //Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            //if (bulletRB != null) bulletRB.linearVelocity = Vector2.zero;  // Reset velocity
            Debug.Log("Player spawned bullet at: " + spawnPos + " | Rotation: " + bullet.transform.rotation);
            if (shootSFX != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSFX);
            }
        }
        else Debug.LogError("BulletPrefab or FirePoint null!");
    }

}