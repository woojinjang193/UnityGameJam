using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    private PlayerInput playerInput;
    private InputAction cancelAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cancelAction = playerInput.actions["Cancel"];
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

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
