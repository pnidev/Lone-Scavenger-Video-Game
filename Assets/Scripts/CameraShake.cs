using UnityEngine;
using System.Collections;
//using Cinemachine;

public class CameraShake : MonoBehaviour
{
  
    public GameObject cam1VirtualCam;
    public static CameraShake Instance;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        if (cam1VirtualCam != null)
            cam1VirtualCam.SetActive(false);  // Disable Cam1 tạm thời


        Debug.Log("Shake start: duration=" + duration + ", magnitude=" + magnitude);  // Debug check chạy
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        
    }

    // Gọi nhanh từ nơi khác
    public void StartShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }
    public void ShakeStrong()
    {
        StartCoroutine(Shake(3f, 0.3f)); // Rung 1 giây, mạnh vừa
    }
}
