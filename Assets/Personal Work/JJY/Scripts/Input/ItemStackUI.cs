using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ItemStackUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image stackImage;
    [SerializeField] private TextMeshProUGUI stackText;

    [Header("Stack Info")]
    [SerializeField] private int maxCount = 5;
    private int _itemCount;

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
        if (Manager.Game.CurStage != 6) return;
        ItemCount = 0;
        StackImageUpdate();
        //이벤트 구독
        //PlayerController.OnItemCountChanged += HandleItemCountChange;
    }
    void OnDisable()
    {
        //이벤트 구독 해제
        //PlayerController.OnItemCountChanged -= HandleItemCountChange;
    }
    private void HandleItemCountChange(int amountChange)
    {
        ItemCount += amountChange;
    }

    private void StackImageUpdate()
    {
        stackImage.fillAmount = (float)ItemCount / maxCount;
        // if (stackText != null)
        // {
        //     stackText.text = ItemCount.ToString();
        //     stackText.text = ItemCount.Tostring() / maxCount;    
        // }
    }
}
