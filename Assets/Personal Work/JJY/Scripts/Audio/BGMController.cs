using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour
{
    [Header("BGM Clips")]
    [SerializeField] private AudioClip title;
    [SerializeField] private AudioClip stage0to2;
    [SerializeField] private AudioClip stage3to6;
    [SerializeField] private AudioClip stage7;
    [SerializeField] private AudioClip stage89;
    [SerializeField] private AudioClip stage10;

    private AudioClip currentBGM = null;

    private void Start()
    {
        // 처음에는 Title BGM
        PlayBGMForScene(-1);

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

        if (value == -1) nextBGM = title;
        else if (value >= 0 && value < 3) nextBGM = stage0to2;
        else if (value >= 3 && value < 7) nextBGM = stage3to6;
        else if (value == 7) nextBGM = stage7;
        else if (value >= 8 && value < 10) nextBGM = stage89;
        else if (value == 10) nextBGM = stage10;
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
