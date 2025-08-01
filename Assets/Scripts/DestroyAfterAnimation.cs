using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            // Lấy độ dài của state animation hiện tại (layer 0) và destroy sau đó
            float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, animLength);
        }
        else
        {
            // Fallback nếu không có Animator: destroy sau 1 giây (adjust nếu cần)
            Destroy(gameObject, 0.6f);
            Debug.LogWarning("No Animator found on " + gameObject.name + ". Using fallback destroy timer.");
        }
    }
}