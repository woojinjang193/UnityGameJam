using UnityEngine;
using DG.Tweening;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float currentTime = 20;
    private Tweener tweener;

    void Awake()
    {
        Manager.Game.OnPlayerStart += StartCountdown;
        Manager.Game.OnPlayerDied += ResetTimer;

        if (timerText == null)
        {
            GameObject timerObject = GameObject.Find("Timer");
            if (timerObject != null)
            {
                timerText = timerObject.GetComponent<TextMeshProUGUI>();
            }
        }
        if (timerText == null)
        {
            Debug.LogError("Timer 찾을 수 없음.");
            return;
        }

        UpdateTimerText(currentTime);
        ResetTimer();
    }

    private void OnDisable()
    {
        Manager.Game.OnPlayerStart -= StartCountdown;
        Manager.Game.OnPlayerDied -= ResetTimer;
    }
    // 죽었을때 사용
    public void ResetTimer()
    {
        //if (tweener != null && tweener.IsActive())
        //{
        //    tweener.Kill();
        //}
        tweener.Kill();
        currentTime = 20;
        //Debug.Log($"리셋리셋 {currentTime}");
        UpdateTimerText(currentTime);
    }
    // 입력시 사용
    public void StartCountdown()
    {
        Debug.Log("타이머 시작");

        tweener = DOTween.To(
            () => currentTime,
            x => currentTime = x,
            0.00f,
            20
        )
        .SetEase(Ease.Linear)
        .OnUpdate(OnTimerUpdate)
        .OnComplete(OnTimerComplete);
    }

    private void OnTimerUpdate()
    {
        UpdateTimerText(currentTime);
    }

    private void UpdateTimerText(float time)
    {
        timerText.text = time.ToString("N2");
    }

    private void OnTimerComplete()
    {
        UpdateTimerText(0.00f);
        Manager.Game.KillPlayer();
        Debug.Log("타이머 종료");
    }
    void OnDestroy()
    {
        if (tweener != null && tweener.IsActive())
        {
            tweener.Kill();
        }
    }
}