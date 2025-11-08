using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Manager
{
    public static GameManager Game => GameManager.GetInstance();
    public static AudioManager Audio => AudioManager.GetInstance();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        GameManager.CreateManager();
        AudioManager.CreateManager();
    }
}
