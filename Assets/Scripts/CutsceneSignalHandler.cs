using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;
using UnityEngine.Video;  // THÊM: Để dùng VideoPlayer
using UnityEngine.UI;     // THÊM: Để dùng RawImage
using System.Collections.Generic; // THÊM: Để dùng List nếu cần
using System.Collections; // THÊM: Để dùng Coroutine nếu cần


public class CutsceneSignalHandler : MonoBehaviour
{
    public PlayableDirector director; // Assign trong Inspector hoặc GetComponent
    public TimelineAsset timeline; // Timeline asset của bạn (assign trong Inspector)
    public GameObject explosionPrefab; // Prefab của explosion (assign trong Inspector nếu dùng instantiate)

    private GameObject explosionObject; // Instance của explosion

    // THÊM: Cho video MP4
    public VideoPlayer videoPlayer;  // Assign VideoPlayer component trong Inspector
    public VideoClip transitionVideo;  // Assign video clip (MP4) trong Inspector
    public GameObject rawImageObject;  // Assign RawImage UI full screen (inactive ban đầu) trong Inspecto
    void Awake()
    {
        director = GetComponent<PlayableDirector>();
        if (director != null)
        {
            // Set bindings trước khi play để tránh mất khi load scene
            SetTimelineBindings();

            // Play timeline (nếu bạn muốn auto-play; nếu không, gọi thủ công)
            director.Play();

            // Đăng ký event stopped
            director.stopped += OnCutsceneFinished;
        }
    }

    // Hàm set bindings dynamically
    private void SetTimelineBindings()
    {
        // Đảm bảo explosion tồn tại
        if (explosionObject == null)
        {
            // Instantiate nếu cần (thay "ExplosionPrefab" bằng path nếu dùng Resources.Load)
            explosionObject = Instantiate(explosionPrefab, transform); // Hoặc vị trí khác
            // Nếu explosion cần persist qua scenes: DontDestroyOnLoad(explosionObject);
        }

        // Duyệt qua tất cả tracks và set binding
        foreach (TrackAsset track in timeline.GetOutputTracks())
        {
            //// Bind cho Activation Track (bind GameObject để bật/tắt)
            //if (track.name == "Activation Track (x)")  // Thay bằng tên thật của Activation Track
            //{
            //    director.SetGenericBinding(track, explosionObject);
            //}

            // Bind cho Animation Track (bind Animator để chơi animation)
            if (track is AnimationTrack && track.name == "ExplosionTrack")  // Thay bằng tên thật của Animation Track
            {
                director.SetGenericBinding(track, explosionObject.GetComponent<Animator>());
            }

            // Thêm binding cho các track khác nếu cần (ví dụ Audio Track bind AudioSource, v.v.)
        }
    }

    void OnCutsceneFinished(PlayableDirector dir)
    {
        // Load scene mới sau khi finished
        StartCoroutine(PlayVideoAndLoadPhase2());
    }
    IEnumerator PlayVideoAndLoadPhase2()
    {
        if (rawImageObject != null) rawImageObject.SetActive(true);  // Hiện RawImage full screen

        // Cấu hình VideoPlayer
        videoPlayer.clip = transitionVideo;
        videoPlayer.isLooping = false;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);  // Render vào texture
        rawImageObject.GetComponent<RawImage>().texture = videoPlayer.targetTexture;  // Gán texture cho RawImage

        // Video có sound tự động (nếu MP4 có audio)
        videoPlayer.Play();

        yield return new WaitForSeconds(4f);  // CHỈNH: Chờ 4s (thay 10s)

        if (rawImageObject != null) rawImageObject.SetActive(false);  // Ẩn RawImage
                                                                      // NGĂN Cutscene hiển thị lại khung hình cuối
        director.time = 0;
        director.Stop();
        // Load Phase2 sau video
        SceneManager.LoadScene("Phase2Scene");
    }
    void OnDestroy()
    {
        if (director != null)
        {
            director.stopped -= OnCutsceneFinished;
        }
    }
}