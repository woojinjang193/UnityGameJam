using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button settingButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button settingSoundButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button close1Button;
    [SerializeField] private Button close2Button;
    [SerializeField] private Button close3Button;

    [Header("Tutorial UI")]
    // [SerializeField] private GameObject tutorialUI;

    [SerializeField] private GameObject[] settingPanels;
    private PlayerInput playerInput;
    private InputAction cancelAction;
    private PanelController settingMenuPanelController;
    private PanelController tutorialPanelController;
    private PanelController soundSettingPanelController;


    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cancelAction = playerInput.actions["Cancel"];
        // if (Manager.Game.CurStage == 1) tutorialUI.gameObject.SetActive(true);
        // else tutorialUI.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        cancelAction.performed += OnCancel;
        settingMenuPanelController = settingPanels[0].GetComponent<PanelController>();
        tutorialPanelController = settingPanels[1].GetComponent<PanelController>();
        soundSettingPanelController = settingPanels[2].GetComponent<PanelController>();

        settingButton.onClick.AddListener(OpenSettingMenuPanel);
        newGameButton.onClick.AddListener(NewGame);
        continueButton.onClick.AddListener(OnClickClosePanel);
        tutorialButton.onClick.AddListener(OpentutorialgPanel);
        settingSoundButton.onClick.AddListener(OpenSoundSettingPanel);
        exitButton.onClick.AddListener(ExtiToTitle);
        close1Button.onClick.AddListener(OnClickClosePanel);
        close2Button.onClick.AddListener(OnClickClosePanel);
        close3Button.onClick.AddListener(OnClickClosePanel);
    }

    void OnDisable()
    {
        cancelAction.performed -= OnCancel;

        settingButton.onClick.RemoveListener(OpenSettingMenuPanel);
        newGameButton.onClick.RemoveListener(NewGame);
        continueButton.onClick.RemoveListener(OnClickClosePanel);
        tutorialButton.onClick.RemoveListener(OpentutorialgPanel);
        settingSoundButton.onClick.RemoveListener(OpenSoundSettingPanel);
        exitButton.onClick.RemoveListener(ExtiToTitle);
        close1Button.onClick.RemoveListener(OnClickClosePanel);
        close2Button.onClick.RemoveListener(OnClickClosePanel);
        close3Button.onClick.RemoveListener(OnClickClosePanel);
    }
    public void OpenSettingMenuPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        settingMenuPanelController.OpenPanel();
    }
    public void OpentutorialgPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        tutorialPanelController.OpenPanel();
    }
    public void OpenSoundSettingPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        soundSettingPanelController.OpenPanel();
    }

    public void OnClickClosePanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        ClosePanel();
    }
    public void ClosePanel()
    {
        settingMenuPanelController.ClosePanel();
        tutorialPanelController.ClosePanel();
        soundSettingPanelController.ClosePanel();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        foreach (GameObject go in settingPanels)
        {
            if (go.activeSelf)
            {
                go.GetComponent<PanelController>().ClosePanel();
            }
        }
    }
    public void NewGame()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        SceneManager.LoadScene("Title");
    }
    public void ExtiToTitle()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        SceneManager.LoadScene("TestMap");
        Manager.Game.InitLevel();
    }
}
