using UnityEngine;

public class YSort : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // y값이 작을수록 (화면 아래쪽일수록) 앞에 그려짐
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
