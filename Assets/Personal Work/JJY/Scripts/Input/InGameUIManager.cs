using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private Button optionButton;
    [SerializeField] private GameObject optionsPanel;
    private PlayerInput playerInput;
    private InputAction cancelAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cancelAction = playerInput.actions["Cancel"];
        optionButton.onClick.AddListener(OpenSettings);
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
        if (optionsPanel.activeSelf)
        {
            CloseSettings();
        }
    }

    public void OpenSettings()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        optionsPanel.SetActive(false);
    }
}
