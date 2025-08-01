using UnityEngine;

public class ObstacleHitbox : MonoBehaviour
{
    private ObstacleBase parentObstacle;

    void Start()
    {
        parentObstacle = GetComponentInParent<ObstacleBase>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentObstacle.SendMessage("OnTriggerEnter2D", other);
        }
    }
}
