using System.Collections;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    private Coroutine goNextCor;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (goNextCor != null)
                goNextCor = StartCoroutine(GoNextStage());
        }
    }

    private IEnumerator GoNextStage()
    {
        yield return new WaitForSeconds(1.5f);
        // 다음 스테이지 가는 함수 작성
    }
}
