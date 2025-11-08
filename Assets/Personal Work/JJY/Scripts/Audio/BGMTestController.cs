using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMTestController : MonoBehaviour
{
    [Header("BGM Clips (씬별 설정)")]
    public AudioClip mainMenuBGM;
    public AudioClip stage1BGM;
    public AudioClip stage2BGM;

    private void Start()
    {
        // 현재 씬에 맞는 브금 재생
        PlayBGMForScene(SceneManager.GetActiveScene().name);

        // 씬이 바뀔 때마다 자동으로 브금 전환
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        PlayBGMForScene(newScene.name);
    }

    private void PlayBGMForScene(string sceneName)
    {
        if (Manager.Audio == null) return;

        switch (sceneName)
        {
            case "MainMenu":
                Manager.Audio.PlayBGM(mainMenuBGM, true, 1f);
                break;
            case "Stage1":
                Manager.Audio.PlayBGM(stage1BGM, true, 1f);
                break;
            case "Stage2":
                Manager.Audio.PlayBGM(stage2BGM, true, 1f);
                break;
            default:
                Manager.Audio.StopBGM(0.5f);
                break;
        }
    }
}
