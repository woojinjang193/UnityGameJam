using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RotateBanana : MonoBehaviour
{
    [SerializeField] Button banana;
    private RectTransform rectTransform;
    [SerializeField] private float rotateTime;
    private Tweener tweener;

    void Start()
    {
        rectTransform = banana.GetComponent<RectTransform>();
        RotateOnStart();
    }

    private void RotateOnStart()
    {
        tweener = rectTransform.DORotate(new Vector3(0, 0, 360), 1.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1)
            .SetRelative();
    }

    public void RotateOnClick()
    {
        tweener.Kill();

        rectTransform.DORotate(new Vector3(0, 0, 360), rotateTime, RotateMode.Fast)
            .SetEase(Ease.OutSine)
            .SetRelative()
            .OnComplete(() =>
            {
                RotateOnStart();
            });
    }

}
