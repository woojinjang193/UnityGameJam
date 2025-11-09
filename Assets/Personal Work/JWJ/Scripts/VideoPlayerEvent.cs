using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerEvent : MonoBehaviour
{
    [SerializeField] private GameObject _city;
    [SerializeField] private GameObject _cloud;
    [SerializeField] private GameObject _fadeIn;
    [SerializeField] private GameObject _fadeOut;
    private VideoPlayer _vp;

    private void Awake()
    {
        _vp = GetComponent<VideoPlayer>();
    }
    void Start()
    {
        _vp.loopPointReached += OnVideoEnd;
        _vp.started += OnVideoStart;
        _vp.errorReceived += OnVideoError;
    }

    void OnVideoStart(VideoPlayer vp)
    {
        Debug.Log("영상 시작");
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("영상 종료");
        _fadeIn.SetActive(true);
        _fadeOut.SetActive(false);
        _cloud.SetActive(true);
        _city.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError($"비디오 오류: {message}");
    }
}
