using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChargeHoldInput : MonoBehaviour
{
    [Header("Input")]
    public InputActionProperty chargeHoldAction;

    [Header("TryNewGame")]
    [SerializeField] private Image fillImage;
    [SerializeField] private float holdDuration;

    private Tween tween;

    void OnEnable()
    {
        chargeHoldAction.action.started += OnChargeStarted;
        chargeHoldAction.action.canceled += OnChargeCancle;
        chargeHoldAction.action.performed += OnChargePerformed;
        chargeHoldAction.action.Enable();
    }
    void OnDisable()
    {
        chargeHoldAction.action.started -= OnChargeStarted;
        chargeHoldAction.action.canceled -= OnChargeCancle;
        chargeHoldAction.action.performed -= OnChargePerformed;
        chargeHoldAction.action.Disable();
        tween.Kill();
    }

    private void OnChargeStarted(InputAction.CallbackContext context)
    {
        tween?.Kill();

        fillImage.gameObject.SetActive(true);
        fillImage.fillAmount = 0;

        tween = fillImage.DOFillAmount(1f, holdDuration)
        .SetEase(Ease.Linear)
        .SetUpdate(true);
    }
    private void OnChargeCancle(InputAction.CallbackContext context)
    {
        tween.Kill();
        ResetFillImage();
    }
    private void OnChargePerformed(InputAction.CallbackContext context)
    {
        tween.Kill();
        SceneManager.LoadScene("map");
        ResetFillImage();
    }
    private void ResetFillImage()
    {
        fillImage.fillAmount = 0;
        fillImage.gameObject.SetActive(false);
    }
}
