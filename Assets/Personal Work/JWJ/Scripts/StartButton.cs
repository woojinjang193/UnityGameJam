using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
        Manager.Game.LevelUp();
        SceneManager.LoadScene("map");
    }
}
