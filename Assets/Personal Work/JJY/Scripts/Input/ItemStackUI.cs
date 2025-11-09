using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ItemStackUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image stackImage;

    [Header("Stack Info")]
    [SerializeField] private int maxCount = 4;

    [Header("DOTween Setting")]
    [SerializeField] private float fillDuration = 0.2f;

    private int _itemCount;
    private Tween fillTween;

    public int ItemCount
    {
        get => _itemCount;
        set
        {
            if (_itemCount == value) return;
            _itemCount = math.clamp(value, 0, maxCount);
            StackImageUpdate();
        }
    }
    void Awake()
    {
        if (Manager.Game.CurStage != 8) return;
        Debug.Log($"{Manager.Game}");
        Manager.Game.OnCoinCountChanged += HandleCoinCountChange;
        ItemCount = Manager.Game.Coin;
        // StackImageUpdate();
    }
    void OnDestroy()
    {
        fillTween.Kill();
        Manager.Game.OnCoinCountChanged -= HandleCoinCountChange;
    }
    private void HandleCoinCountChange(int coinCount)
    {
        Debug.Log("이벤트");
        ItemCount = coinCount;
    }

    public void StackImageUpdate()
    {
        fillTween.Kill();
        float targetFill = (float)_itemCount / maxCount;

        fillTween = stackImage.DOFillAmount(targetFill, fillDuration)
        .SetEase(Ease.OutSine)
        .SetUpdate(true);
    }
    //
    public void StackItemCount()
    {
        ItemCount++;
    }
}
