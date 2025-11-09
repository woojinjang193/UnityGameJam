using UnityEngine;

public class YSortTree : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bottom; // 기둥/밑동
    [SerializeField] private SpriteRenderer top;    // 윗부분(나뭇잎)

    void Start()
    {
        top = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        bottom = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        // 트리 기준 YSort (트리 발이 닿는 지점에 맞춰서 pivot 잡아두면 좋음)
        int baseOrder = Mathf.RoundToInt(-transform.position.y * 100);

        // 밑동: 플레이어랑 같은 룰
        bottom.sortingOrder = baseOrder;

        // 윗부분: 항상 밑동보다 앞(위에서 덮는 느낌)
        top.sortingOrder = baseOrder + 1;
    }
}
