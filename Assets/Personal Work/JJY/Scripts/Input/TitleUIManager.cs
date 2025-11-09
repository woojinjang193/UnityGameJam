using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField] private Button settingButton1;
    [SerializeField] private Button settingButton2;
    [SerializeField] private Button closeSettingButton;
    [SerializeField] private GameObject settingsPanel;
    private PanelController panelController;
    private PlayerInput playerInput;
    private InputAction cancelAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        panelController = settingsPanel.GetComponent<PanelController>();
        cancelAction = playerInput.actions["Cancel"];
        settingButton1.onClick.AddListener(OpenSettings);
        settingButton2.onClick.AddListener(OpenSettings);
    }

    void OnEnable()
    {
        cancelAction.performed += OnCancel;
        closeSettingButton.onClick.AddListener(OnClickClosePanel);
    }

    void OnDisable()
    {
        cancelAction.performed -= OnCancel;
        closeSettingButton.onClick.RemoveListener(OnClickClosePanel);
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (settingsPanel.activeSelf)
        {
            CloseSettings();
        }
    }

    public void OpenSettings()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        panelController.OpenPanel();
    }

    private void OnClickClosePanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        CloseSettings();
    }
    public void CloseSettings()
    {
        panelController.ClosePanel();
    }
}
