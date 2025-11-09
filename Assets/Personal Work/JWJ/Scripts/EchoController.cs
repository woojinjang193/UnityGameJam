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
    private BoxInteraction _box;

    private bool _isPushing => _boxRb != null && _isKeyPressed && _curInput.sqrMagnitude > 0f;

    //방향 잠금
    private bool _faceLocked = false;
    private string _lockedDir = "S";
    private bool _wasPushing = false;
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

        _faceLocked = false;
        _lockedDir = "S";
        _wasPushing = false;
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
        _box = null;

        _faceLocked = false;
        _lockedDir = "S";
        _wasPushing = false;
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
        _box = null;

        _faceLocked = false;
        _lockedDir = "S";
        _wasPushing = false;
    }

    private void FixedUpdate()
    {
        _speed = _isPushing ? 3.5f : 7f;

        if (!_isPlaying)
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
            if (_box != null)
            {
                _box.SetMovable(false);
            }
            
            string dir = beforedic;
            if (_curInput.x <= -0.5f) dir = "W";
            else if (_curInput.x >= 0.5f) dir = "E";
            else if (_curInput.y >= 0.5f) dir = "N";
            else if (_curInput.y <= -0.5f) dir = "S";
            beforedic = dir;
        }

        if (_isPushing && !_wasPushing)
        {
            _lockedDir = beforedic;
            _faceLocked = true;
        }
        else if (!_isPushing && _wasPushing)
        {
            _faceLocked = false;
        }
        _wasPushing = _isPushing;

        string dirForAnim = _faceLocked ? _lockedDir : beforedic;
        string statePrefix = (_curInput.sqrMagnitude > 0f) ? "Walk" : "Idle";
        string target = $"{statePrefix}{dirForAnim}";
        var st = animator.GetCurrentAnimatorStateInfo(0);
        if (!st.IsName(target)) animator.Play(target);
    }
    public void SetInteractPressed(bool pressed)
    {
        _isKeyPressed = pressed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Box")) return;

        var box = other.GetComponent<BoxInteraction>();
        if (box == null) return;

        // 자기 ID와 같은 박스만 밀 수 있음
        if (_isKeyPressed && !box.IsPlayerOnly && box.OwnerId == _echoID)
        {
            _boxRb = other.attachedRigidbody;
            _box = box;
            _box.SetMovable(true);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fire"))
        {
            _box = null;
            _boxRb = null;
            _faceLocked = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Box")) return;

        if (!_isKeyPressed) //키 안누르고 있을 때만 놓기
        {
            if(_box != null)
            {
                _box.SetMovable(false);
            }
            
            _boxRb = null;
            _box = null;
            _faceLocked = false;
            animator.Play($"Idle{beforedic}");
        }
    }
}
