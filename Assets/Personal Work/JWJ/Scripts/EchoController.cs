using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoController : MonoBehaviour
{
    [SerializeField] private int _echoID;
    [SerializeField] private float _speed = 7f;
    private List<InputRecord> _records;
    private int _curInputIndex;
    
    private Rigidbody2D _rigid;
    private Vector2 _curInput = Vector2.zero;

    private bool _isPlaying = false;
    private float _startTime;

    private Color _startColor;
    private SpriteRenderer _sr;

    private bool _isKeyPressed;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _startColor = _sr.color;
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
    }
    public void ResetToSpawn(Vector2 spawnPoint)
    {
        transform.position = spawnPoint;
        _rigid.linearVelocity = Vector2.zero;
        _curInputIndex = 0;
        _curInput = Vector2.zero;
        _isPlaying = false;
        _isKeyPressed = false;
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
            _rigid.position = _records[_curInputIndex].Position;
            //Debug.Log($"[Echo {_echoID}] 입력 적용: Elapsed={elapsed}, RecordTime={_records[_curInputIndex].Time}, Input={_curInput}, Position={_rigid.position}");
            _curInputIndex++;
        }

        Vector2 nextVec = _curInput * _speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec);
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
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            _speed = 7f;
        }
    }
}
