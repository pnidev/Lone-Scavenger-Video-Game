using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneBossController : MonoBehaviour
{
    public PlayableDirector director;  // Kéo Playable Director component (từ CutsceneDirector) vào Inspector
    public CameraShake cameraShake;    // Kéo Main Camera (có CameraShake) vào Inspector

    void Start()
    {
        if (director == null) director = GetComponent<PlayableDirector>();  // Auto bind nếu quên kéo

        director.Play();  // Auto play Timeline khi scene start
        director.stopped += OnTimelineStopped;  // Gọi khi end 10s

        StartCoroutine(TriggerShakeAt4s());  // Trigger rung thủ công tại 4s
    }

    private IEnumerator TriggerShakeAt4s()
    {
        yield return new WaitForSeconds(4f);  // Chờ 5s từ start
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.ShakeStrong();  // Gọi rung
            Debug.Log("Trigger shake thủ công tại 5s thành công");
        }
        else
        {
            Debug.LogError("CameraShake Instance null - check attach vào Main Camera!");
        }
    }

    private void OnTimelineStopped(PlayableDirector dir)
    {
        // Load scene boss fight sau 10s
        //FindObjectOfType<TransitionManager>()?.LoadNextScene("BossFight");
    }
}