using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KillButton : MonoBehaviour
{
    [Header("Input")]
    public InputActionProperty chargeHoldAction;

    [Header("TryNewGame")]
    [SerializeField] private Image fillImage;
    [SerializeField] private float holdDuration;
    [SerializeField] private float _coolTime;

    private bool _ready = true;

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
        if (!_ready) return;
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
        if(_ready)
        {
            _ready = false;
            tween.Kill();
            Manager.Game.KillPlayer();
            ResetFillImage();
            StartCoroutine(CoolDown());
        }
        
    }
    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(_coolTime);
        _ready = true;
    }
    private void ResetFillImage()
    {
        fillImage.fillAmount = 0;
        fillImage.gameObject.SetActive(false);
    }
}
