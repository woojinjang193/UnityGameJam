using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Manager
{
    public static GameManager Game => GameManager.GetInstance();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        GameManager.CreateManager();
    }
}
