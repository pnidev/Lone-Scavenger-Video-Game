using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    public float speed = 8f; // Tốc độ bay trái
    public GameObject explosionAnimPrefab; // Kéo ExplosionAnimPrefab
    //public float upSpeed = 0.5f;

    private Animator animator;
    private Vector2 direction;
    public float yOffset = 0.5f;

    public AudioClip explodeSound; // 🎵 Âm thanh va chạm
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("[BulletBoss] Animator null!");
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        Invoke("EnableCollider", 0.1f);
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector2 targetPos = player.transform.position;
            targetPos.y += yOffset;
            direction = (targetPos - (Vector2)transform.position).normalized;
        }
        else
        {
            direction = -transform.right; // Bay trái nếu không có player
        }
    }
    void EnableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
    }

    void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime);
        Debug.Log("BulletBoss Pos: " + transform.position + " | Dir: " + direction);

        // Huỷ nếu bay ra khỏi màn
        if (transform.position.x < -30f || transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {

            // 🎵 Phát âm thanh nổ
            if (explodeSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(explodeSound);
            }

            Player player = other.GetComponent<Player>();
                if (player != null && !player.isJumping) // Trúng nếu không nhảy
                {
                    player.LoseLife();
                    Instantiate(explosionAnimPrefab, transform.position, Quaternion.identity); // Explosion animation
                }
                Destroy(gameObject);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) // Check layer ground
            {
                Instantiate(explosionAnimPrefab, transform.position, Quaternion.identity); // Nổ khi hit ground
                Destroy(gameObject);
            }
        }
    }