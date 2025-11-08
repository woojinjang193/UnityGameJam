using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct InputRecord
{
    public float Time;
    public Vector2 Input;
    public Vector2 Position;
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 7;

    private Vector2 _inputVec;
    private Rigidbody2D _rigid;
    private int _dieCount = 0;
    private Vector2 _lastRecordedInput = Vector2.positiveInfinity;

    private List<InputRecord> _records = new();

    private bool _isRecording = false;
    private bool _isPlaying = false;
    private float _recordStartTime;
    private Color _startColor;
    private SpriteRenderer _spriteRenderer;

    private bool _isKeyPressed;

    private bool _canMoveBox = false;
    private GameObject _box;
    private Rigidbody2D _boxRb;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _startColor = _spriteRenderer.color;
    }
    private void OnEnable()
    {
        _records.Clear();
        _isRecording = false;
        _isPlaying = false;
        _lastRecordedInput = Vector2.positiveInfinity;
        _inputVec = Vector2.zero;
        _startColor.a = 1;
        _spriteRenderer.color = _startColor;
        transform.localScale = new Vector3(1, 1, 1);
    }
    public void OnMove(InputValue value)
    {
        _inputVec = value.Get<Vector2>();

        if (!_isPlaying && _inputVec != Vector2.zero)
        {
            _isPlaying = true;
            _isRecording = true;
            _recordStartTime = Time.fixedTime;

            _records.Add(new InputRecord
            {
                Time = 0f,
                Input = _inputVec,
                Position = _rigid.position
            });
            _lastRecordedInput = _inputVec;

            Manager.Game.StartPlayer();
        }
    }
    public void OnInteract(InputValue value)
    {
        _isKeyPressed = value.isPressed;
    }
    private void FixedUpdate()
    {
        if (_isRecording && _inputVec != _lastRecordedInput)
        {
            _records.Add(new InputRecord
            {
                Time = Time.fixedTime - _recordStartTime,
                Input = _inputVec,
                Position = _rigid.position
            });
            _lastRecordedInput = _inputVec;
            //Debug.Log($"[Player] 입력 기록: Time={Time.fixedTime}, Input={_inputVec}, Position={_rigid.position}");
        }
        Vector2 nextVec = _inputVec * _speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec);

        if (_boxRb != null && _canMoveBox && _isKeyPressed)
        {
            _boxRb.MovePosition(_boxRb.position + nextVec);
        }
    }

    public void DiePlayer()
    {
        if (_isRecording)
        {
            _records.Add(new InputRecord
            {
                Time = Time.fixedTime - _recordStartTime,
                Input = Vector2.zero,
                Position = _rigid.position
            });
        }
        _isRecording = false;
        _inputVec = Vector2.zero;

        _dieCount++;
        Manager.Game.PlayerDieAndSave(_records, gameObject, _dieCount, _recordStartTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            _canMoveBox = true;
            _box = collision.gameObject;
            _boxRb = collision.rigidbody;
            _speed = 3.5f;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            _canMoveBox = false;
            _box = null;
            _boxRb = null;
            _speed = 7f;
        }
    }

    
}
