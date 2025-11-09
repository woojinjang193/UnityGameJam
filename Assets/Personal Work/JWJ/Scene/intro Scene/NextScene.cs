using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public void NextSceneGOGO()
    {
        SceneManager.LoadScene("InGame");
    }
}
