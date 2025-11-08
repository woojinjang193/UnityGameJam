using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneChanger : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("씬 전환: MainMenu");
            SceneManager.LoadScene("MainMenu");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("씬 전환: Stage1");
            SceneManager.LoadScene("Stage1");
        }
    }
}
