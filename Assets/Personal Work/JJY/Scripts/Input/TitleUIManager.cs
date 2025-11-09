using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField] private Button howToButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button closeSettingButton;
    [SerializeField] private Button closeHowToButton;
    [SerializeField] private GameObject howToPanel;
    [SerializeField] private GameObject settingsPanel;
    private PanelController settingpanelController;
    private PanelController howTopanelController;
    private PlayerInput playerInput;
    private InputAction cancelAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        settingpanelController = settingsPanel.GetComponent<PanelController>();
        howTopanelController = howToPanel.GetComponent<PanelController>();
        cancelAction = playerInput.actions["Cancel"];
        howToButton.onClick.AddListener(OpenHowToPlay);
        settingButton.onClick.AddListener(OpenSettings);
    }

    void OnEnable()
    {
        cancelAction.performed += OnCancel;
        closeSettingButton.onClick.AddListener(OnClickCloseSettingPanel);
        closeHowToButton.onClick.AddListener(OnClickCloseHowToPanel);
    }

    void OnDisable()
    {
        cancelAction.performed -= OnCancel;
        closeSettingButton.onClick.RemoveListener(OnClickCloseSettingPanel);
        closeHowToButton.onClick.RemoveListener(OnClickCloseHowToPanel);
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (settingsPanel.activeSelf)
        {
            CloseSettings();
        }
        if (howToPanel.activeSelf)
        {
            CloseHowTo();
        }
    }

    public void OpenSettings()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        settingpanelController.OpenPanel();
    }
    public void OpenHowToPlay()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        howTopanelController.OpenPanel();
    }

    private void OnClickCloseSettingPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        CloseSettings();
    }
    private void OnClickCloseHowToPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        CloseHowTo();
    }
    public void CloseSettings()
    {
        settingpanelController.ClosePanel();
    }
    public void CloseHowTo()
    {
        howTopanelController.ClosePanel();
    }
}
