using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource audioSource;
    public AudioClip sharedBGM;

    // Danh sách các scene được phép phát nhạc
    public string[] allowedScenes = { "Phase1", "Cutscene", "Phase2" };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded; // Bắt sự kiện đổi scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsSceneAllowed(scene.name))
            PlaySharedBGM();
        else
            StopBGM();
    }

    bool IsSceneAllowed(string sceneName)
    {
        foreach (string allowed in allowedScenes)
        {
            if (sceneName == allowed)
                return true;
        }
        return false;
    }

    public void PlaySharedBGM()
    {
        Debug.Log("🟢 PlaySharedBGM được gọi");
        if (audioSource.clip == sharedBGM && audioSource.isPlaying) return;
        audioSource.Stop();
        audioSource.clip = sharedBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }
}
