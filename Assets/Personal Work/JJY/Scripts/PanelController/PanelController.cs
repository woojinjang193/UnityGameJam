using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    [SerializeField] private float duration;
    private Tween currentTween;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        currentTween.Kill(true);
    }
    public void OpenPanel()
    {
        currentTween.Kill(true);
        gameObject.SetActive(true);

        rect.localScale = Vector2.zero;
        currentTween = rect.DOScale(1f, duration)
        .SetEase(Ease.OutBack)
        .SetUpdate(true);
    }
    public void ClosePanel()
    {
        currentTween.Kill(true);
        currentTween = rect.DOScale(0f, duration)
        .SetEase(Ease.InQuint)
        .SetUpdate(true)
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

}
