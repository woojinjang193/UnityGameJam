using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoController : MonoBehaviour
{
    [SerializeField] private int _echoID;
    [SerializeField] private float _speed = 7f;
    private List<InputRecord> _records;
    private int _curInputIndex;

    public int EchoID => _echoID;

    private Rigidbody2D _rigid;
    private Vector2 _curInput = Vector2.zero;

    private bool _isPlaying = false;
    private float _startTime;

    private Color _startColor;
    private SpriteRenderer _sr;

    private bool _isKeyPressed;
    private Rigidbody2D _boxRb;

    private string beforedic = "S";
    private Animator animator;

    private bool _isPushing => _boxRb != null && _isKeyPressed && _curInput.sqrMagnitude > 0f;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _startColor = _sr.color;
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Manager.Game.OnPlayerStart += OnPlayerStart;
        Manager.Game.OnPlayerDied += OnPlayerDied;

        _startColor.a = 0.3f;
        _sr.color = _startColor;
    }

    private void OnDisable()
    {
        Manager.Game.OnPlayerStart -= OnPlayerStart;
        Manager.Game.OnPlayerDied -= OnPlayerDied;
    }
    private void OnPlayerStart()
    {
        _startTime = Time.fixedTime;
        _isPlaying = true;
    }
    private void OnPlayerDied()
    {
        _curInputIndex = 0;
        _curInput = Vector2.zero;
    }
    public void Init(List<InputRecord> record, int id, float recordStartTime)
    {
        _echoID = id;
        _records = new List<InputRecord>(record);

        _curInputIndex = 0;
        _curInput = Vector2.zero;

        _isPlaying = false;
        _isKeyPressed = false;
        _startTime = recordStartTime;
        _boxRb = null;
    }
    public void ResetToSpawn(Vector2 spawnPoint)
    {
        transform.position = spawnPoint;
        _rigid.linearVelocity = Vector2.zero;
        _curInputIndex = 0;
        _curInput = Vector2.zero;
        _isPlaying = false;
        _isKeyPressed = false;
        _boxRb = null;
    }

    private void FixedUpdate()
    {
        if(!_isPlaying)
        {
            return;
        }

        if (_curInputIndex >= _records.Count)
        {
            _rigid.linearVelocity = Vector2.zero;
            return;
        }

        float elapsed = Time.fixedTime - _startTime;

        // while로 변경하여 현재 시간까지의 모든 입력을 처리
        while (_curInputIndex < _records.Count && elapsed >= _records[_curInputIndex].Time)
        {
            _curInput = _records[_curInputIndex].Input;
            _isKeyPressed = _records[_curInputIndex].Interact;
            _rigid.position = _records[_curInputIndex].Position;
            //Debug.Log($"[Echo {_echoID}] 입력 적용: Elapsed={elapsed}, RecordTime={_records[_curInputIndex].Time}, Input={_curInput}, Position={_rigid.position}");
            _curInputIndex++;
        }

        Vector2 nextVec = _curInput * _speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec);

        if (_isPushing)
        {
            _boxRb.MovePosition(_boxRb.position + nextVec);
        }

        if (!_isPushing)
        {
            string dir = beforedic;
            if (_curInput.x <= -0.5f) dir = "W";
            else if (_curInput.x >= 0.5f) dir = "E";
            else if (_curInput.y >= 0.5f) dir = "N";
            else if (_curInput.y <= -0.5f) dir = "S";
            beforedic = dir;
        }

        string statePrefix = (_curInput.magnitude > 0)
            ? (_isPushing ? "Push" : "Walk")
            : "Idle";

        string target = $"{statePrefix}{beforedic}";
        var st = animator.GetCurrentAnimatorStateInfo(0);
        if (!st.IsName(target))
            animator.Play(target);
    }
    public void SetInteractPressed(bool pressed)
    {
        _isKeyPressed = pressed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            _speed = 3.5f;
            _boxRb = collision.rigidbody;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            _speed = 7f;
            _boxRb = null;
            animator.Play($"Idle{beforedic}");
        }
    }
}
