using UnityEngine;
using DG.Tweening;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float currentTime = 20;
    private Tweener tweener;
    private Tween lastStageTweenr;

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
        ResetTimer();
        Manager.Game.OnPlayerStart -= StartCountdown;
        Manager.Game.OnPlayerDied -= ResetTimer;
    }
    // 죽었을때 사용
    public void ResetTimer()
    {
        tweener.Kill();
        lastStageTweenr.Kill();
        timerText.color = Color.white;
        currentTime = 20;
        UpdateTimerText(currentTime);
    }
    // 입력시 사용
    public void StartCountdown()
    {
        Debug.Log("타이머 시작");

        if (Manager.Game.CurStage < 8)
        {
            StartNormalTimer();
        }
        else
        {
            StartLastStageTimer();
        }
        
    }
    private void StartNormalTimer()
    {
        tweener.Kill();
        lastStageTweenr.Kill();
        tweener = DOTween.To(
            () => currentTime,
            x => currentTime = x,
            0.00f,
            20
        )
        .SetEase(Ease.Linear)
        .OnUpdate(OnTimerUpdate)
        .OnComplete(OnTimerComplete)
        .SetUpdate(true);
    }
    private void StartLastStageTimer()
    {
        tweener.Kill();
        lastStageTweenr.Kill();
        timerText.text = "99:99";
        timerText.color = Color.white;

        lastStageTweenr = DOTween.Sequence()
        .Append(timerText.DOColor(new Color(1f, 1f, 1f, 0f), 0.5f))
        .Append(timerText.DOColor(Color.white, 0.5f))
        .SetLoops(-1, LoopType.Restart)
        .SetUpdate(true);
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
        tweener.Kill();
        lastStageTweenr.Kill();
    }
}