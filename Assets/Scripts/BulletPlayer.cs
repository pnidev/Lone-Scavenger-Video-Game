using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    public float speed = 12f;
    private Animator animator;
    private Rigidbody2D rb;  // Get RB

    public GameObject explosionPrefab;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("[BulletPlayer] Animator null!");
        if (rb == null) Debug.LogError("[BulletPlayer] Rigidbody2D null!");
        Debug.Log("BulletPlayer Start called! Pos: " + transform.position);

        animator.Play("BulletPlayer");

        // Anti-overlap: Disable Collider 0.1s
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        Invoke("EnableCollider", 0.1f);
    }

    void EnableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
        Debug.Log("BulletPlayer Collider enabled!");
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            Vector2 moveDir = transform.right * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDir);  // Use MovePosition for kinematic
            Debug.Log("BulletPlayer FixedUpdate: Moving to Pos " + transform.position + " | Dir " + moveDir);
        }
        else
        {
            transform.Translate(transform.right * speed * Time.fixedDeltaTime);  // Fallback if no RB
        }
        if (transform.position.x > 30f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("BulletPlayer OnTrigger: Hit " + other.name + " | Tag " + other.tag);
        if (other.CompareTag("Boss"))
        {
            Boss boss = other.GetComponent<Boss>();
            if (boss != null) boss.TakeDamage(1);
            InstantiateExplosion();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            Debug.LogWarning("Bullet overlapped with Player collider!");
        }
    }
    private void InstantiateExplosion()
    {
        if (explosionPrefab != null)
        {
            Vector3 effectPos = transform.position;  // Vị trí nổ tại đạn
            Instantiate(explosionPrefab, effectPos, Quaternion.identity);
            Debug.Log("[BulletPlayer] Explosion instantiated at " + effectPos);
        }
        else
        {
            Debug.LogError("[BulletPlayer] explosionPrefab chưa gán!");
        }
    }
}