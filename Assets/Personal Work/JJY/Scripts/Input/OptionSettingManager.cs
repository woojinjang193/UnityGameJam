using UnityEngine;
using UnityEngine.InputSystem;

public class OptionSettingManager : MonoBehaviour
{
    [SerializeField] private GameObject[] settingPanels;
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
        foreach (GameObject go in settingPanels)
        {
            if (go.activeSelf)
            {
                go.SetActive(false);
            }
        }
    }
}
