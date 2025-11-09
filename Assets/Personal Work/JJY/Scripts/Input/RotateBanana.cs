using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class RotateBanana : MonoBehaviour
{
    [SerializeField] Button banana;
    [SerializeField] AudioClip fanfare;
    [SerializeField] private float rotateTime;
    [SerializeField] private ParticleSystem _particle;
    private RectTransform rectTransform;
    private Tweener tweener;
    private int rotateCount = 0;

    void Start()
    {
        rectTransform = banana.GetComponent<RectTransform>();
        RotateOnStart();
        rotateCount = 0;
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
        rotateCount++;

        rectTransform.DORotate(new Vector3(0, 0, 360), rotateTime, RotateMode.Fast)
            .SetEase(Ease.OutSine)
            .SetRelative()
            .OnComplete(() =>
            {
                RotateOnStart();
                Fanfare();
            });
    }

    private void Fanfare()
    {
        if (rotateCount < 20) return;
        AudioManager.Instance.PlaySFX(fanfare);
        _particle.Play();
        rotateCount = 0;
    }

}
