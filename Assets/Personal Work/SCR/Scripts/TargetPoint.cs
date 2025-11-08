using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetPoint : MonoBehaviour
{
    private Coroutine goNextCor;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("충돌");
            if (goNextCor == null)
                goNextCor = StartCoroutine(GoNextStage());
        }
    }

    private IEnumerator GoNextStage()
    {
        yield return new WaitForSeconds(1.5f);
        Manager.Game.LevelUp();
        SceneManager.LoadScene("map");
    }
}
