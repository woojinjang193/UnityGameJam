using UnityEngine;
using UnityEngine.UI;

public class SFXTestButton : MonoBehaviour
{
    public Button button;
    public AudioClip testClip;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (testClip != null)
                AudioManager.Instance.PlaySFX(testClip);
            else
                Debug.LogWarning("SFXTestButton: testClip이 지정되지 않았습니다!");
        });
    }
}
