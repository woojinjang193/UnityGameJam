using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button newGameButton;

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
        newGameButton.onClick.AddListener(NewGame);
    }

    void OnDisable()
    {
        cancelAction.performed -= OnCancel;
        newGameButton.onClick.RemoveListener(NewGame);
    }
    public void OpenPanel()
    {
        GetComponent<PanelController>().OpenPanel();
    }
    public void ClosePanel()
    {
        GetComponent<PanelController>().ClosePanel();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        foreach (GameObject go in settingPanels)
        {
            if (go.activeSelf)
            {
                go.GetComponent<PanelController>().ClosePanel();
                // go.SetActive(false);
            }
        }
    }
    public void NewGame()
    {
        // GetComponent<PanelController>().ClosePanel();
        SceneManager.LoadScene(1);
    }
    public void ExtiToTitle()
    {
        // GetComponent<PanelController>().ClosePanel();
        SceneManager.LoadScene(0);
        Manager.Game.InitLevel();
    }
}
