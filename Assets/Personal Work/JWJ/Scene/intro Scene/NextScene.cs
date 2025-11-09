using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    public void NextSceneGOGO()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
