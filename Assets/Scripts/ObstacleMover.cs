using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [SerializeField]
    public float defaultSpeed = 10f;

    void Update()
    {
        if (MapSpeedController.Instance != null && MapSpeedController.Instance.SpeedMultiplier == 0f) return;
        float multiplier = MapSpeedController.Instance != null ? MapSpeedController.Instance.SpeedMultiplier : 1f;
        float speed = defaultSpeed * multiplier;
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < -30f)
        {
            Destroy(gameObject);
        }
    }
}
