using System.Collections;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    [Header("Ground 체크")]
    public LayerMask groundMask;          // Ground 레이어 설정
    public float checkRadius = 0.05f;     // 발 위치 판정 반경

    [Header("연출")]
    public float fallDelay = 0.06f;       // 순간적인 끊김 방지용 (선택)
    public float fallDuration = 0.35f;    // 실제 떨어지는 연출 시간

    private bool isFalling = false;
    private SpriteRenderer sr;
    private TopDownCharacterController moveController; // 실제 이동 스크립트 넣어줄 예정
    [SerializeField] private Vector3 respawnPos;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        // 여기 본인 이동 스크립트 타입으로 바꿔
        moveController = GetComponent<TopDownCharacterController>();
    }

    private void SetRespawnPos(Vector3 pos)
    {
        respawnPos = pos;
    }

    private void Update()
    {
        if (isFalling) return;

        // 발 밑에 Ground 있는지 확인
        bool onGround = Physics2D.OverlapCircle(transform.position, checkRadius, groundMask);

        if (!onGround)
        {
            StartCoroutine(FallCheckRoutine());
        }
    }

    private IEnumerator FallCheckRoutine()
    {
        if (isFalling) yield break;

        // 잠깐 기다렸다가(경계에서 튕기는 거 방지)
        yield return new WaitForSeconds(fallDelay);

        bool stillNoGround = !Physics2D.OverlapCircle(transform.position, checkRadius, groundMask);
        if (!stillNoGround) yield break;

        // 진짜 떨어짐
        yield return StartCoroutine(FallRoutine());
    }

    private IEnumerator FallRoutine()
    {
        isFalling = true;
        if (moveController != null) moveController.enabled = false; // 입력/이동 막기

        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;
        Color startColor = sr != null ? sr.color : Color.white;

        float t = 0f;
        while (t < fallDuration)
        {
            t += Time.deltaTime;
            float n = t / fallDuration;

            // 구멍 중심 쪽으로 살짝 끌려가게 (선택: 현재 위치 유지해도 됨)
            // transform.position = Vector3.Lerp(startPos, startPos + new Vector3(0, -0.1f, 0), n);

            // 작아지면서 아래로 빠지는 느낌
            float s = Mathf.Lerp(1f, 0.1f, n);
            transform.localScale = startScale * s;

            // 살짝 어두워지거나 투명해짐
            if (sr != null)
            {
                var c = startColor;
                c.a = Mathf.Lerp(1f, 0f, n);
                sr.color = c;
            }

            yield return null;
        }

        // 여기서 죽음/리스폰 처리
        // 예: GameManager.Instance.RespawnPlayer();
        // 또는 시작 위치로 이동 후 초기화

        // 데모용: 원상복구 + 시작 지점 텔레포트
        transform.position = respawnPos; // TODO: 리스폰 위치로 교체
        transform.localScale = new Vector3(1, 1, 1);
        if (sr != null) sr.color = startColor;
        if (moveController != null) moveController.enabled = true;

        isFalling = false;
    }
}
