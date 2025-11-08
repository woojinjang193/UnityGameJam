using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField] private Button settingButton1;
    [SerializeField] private Button settingButton2;
    [SerializeField] private GameObject settingsPanel;
    private PlayerInput playerInput;
    private InputAction cancelAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cancelAction = playerInput.actions["Cancel"];
        settingButton1.onClick.AddListener(OpenSettings);
        settingButton2.onClick.AddListener(OpenSettings);
    }

    void OnEnable()
    {
        cancelAction.performed += OnCancel;
    }

    void OnDisable()
    {
        cancelAction.performed -= OnCancel;
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
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
