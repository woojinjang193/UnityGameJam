using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour
{
    [Header("BGM Clips")]
    [SerializeField] private AudioClip title;
    [SerializeField] private AudioClip stage1to3;
    [SerializeField] private AudioClip stage4to7;
    [SerializeField] private AudioClip stage8;
    [SerializeField] private AudioClip stage910;
    [SerializeField] private AudioClip stage11;

    private AudioClip currentBGM = null;

    private void Start()
    {
        // 처음에는 Title BGM
        PlayBGMForScene(0);

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        PlayBGMForScene(Manager.Game.CurStage);
    }

    private void PlayBGMForScene(int value)
    {
        if (AudioManager.Instance == null) return;

        AudioClip nextBGM = null;

        if (value == 0) nextBGM = title;
        else if (value >= 1 && value <= 3) nextBGM = stage1to3;
        else if (value >= 4 && value <= 7) nextBGM = stage4to7;
        else if (value == 8) nextBGM = stage8;
        else if (value >= 9 && value <= 10) nextBGM = stage910;
        else if (value == 11) nextBGM = stage11;
        else
        {
            if (currentBGM != null)
            {
                AudioManager.Instance.StopBGM(0.5f);
            }
            currentBGM = null;
            return;
        }

        if (nextBGM != null && nextBGM == currentBGM)
        {
            return;
        }
        if (nextBGM != null)
        {
            AudioManager.Instance.PlayBGM(nextBGM, true, 1f);
            currentBGM = nextBGM;
        }
    }
}
