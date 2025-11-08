using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[Serializable]
public struct InputRecord
{
    public float Time;
    public Vector2 Input;
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 7;

    private GameObject _player;
    private Vector2 _inputVec;
    private Rigidbody2D _rigid;
    private int _dieCount = 0;
    private Vector2 _lastRecordedInput = Vector2.positiveInfinity;

    private List<InputRecord> _records = new();

    private bool _isRecording = false;
    private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;
    private float _recordStartTime;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _player = gameObject;
    }
    private void OnEnable()
    {
        _records.Clear();
        _isRecording = false;
        _isPlaying = false;
        _lastRecordedInput = Vector2.positiveInfinity;
        _inputVec = Vector2.zero;
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
                Input = _inputVec
            });
            _lastRecordedInput = _inputVec;

            Manager.Game.StartPlayer();
        }
    }
    private void FixedUpdate()
    {
        if(_isRecording && _inputVec != _lastRecordedInput)
        {
            _records.Add(new InputRecord
            {
                Time = Time.fixedTime - _recordStartTime,
                Input = _inputVec
            });
            _lastRecordedInput = _inputVec;
            Debug.Log($"[Player] 입력 기록: Time={Time.fixedTime}, Input={_inputVec}, Position={_rigid.position}");
        }
        Vector2 nextVec = _inputVec * _speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec);
    }
    
    public void DiePlayer()
    {
        if (_isRecording)
        {
            _records.Add(new InputRecord
            {
                Time = Time.fixedTime - _recordStartTime,
                Input = Vector2.zero
            });
        }
        _isRecording = false;
        _inputVec = Vector2.zero;

        _dieCount++;
        Manager.Game.PlayerDieAndSave(_records, _player, _dieCount);
    }

}
