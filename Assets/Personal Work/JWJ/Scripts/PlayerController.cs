using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct InputRecord
{
    public float Time;
    public Vector2 Input;
    public Vector2 Position;
    public bool Interact;
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 7;

    private Vector2 _inputVec;
    private Rigidbody2D _rigid;
    private int _dieCount = 0;
    private Vector2 _lastRecordedInput = Vector2.positiveInfinity;
    private Animator animator;
    private List<InputRecord> _records = new();

    private bool _isRecording = false;
    private bool _isPlaying = false;
    private float _recordStartTime;
    private Color _startColor;
    private SpriteRenderer _spriteRenderer;

    private bool _isKeyPressed;
    private bool _isPush = false;

    private bool _canMoveBox = false;
    private GameObject _box;
    private Rigidbody2D _boxRb;
    private bool _ending = false;

    //애니메이션 방향 잠금용
    private bool _faceLocked = false;
    private string _lockedDir = "S";
    private bool _wasPushing = false;

    private bool _isPushing => _canMoveBox && _isKeyPressed && _boxRb != null && _inputVec.sqrMagnitude > 0f;

    private string beforedic = "S";


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _startColor = _spriteRenderer.color;
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Manager.Game.OnTransitioning += OnTransitioning;
        _records.Clear();
        _isRecording = false;
        _isPlaying = false;
        _lastRecordedInput = Vector2.positiveInfinity;
        _inputVec = Vector2.zero;
        _startColor.a = 1;
        _spriteRenderer.color = _startColor;
        transform.localScale = new Vector3(1, 1, 1);

        _faceLocked = false;
        _lockedDir = "S";
        _wasPushing = false;
    }
    private void OnDisable()
    {
        Manager.Game.OnTransitioning -= OnTransitioning;
    }

    private void OnTransitioning()
    {
        _isPlaying = true;
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
                Position = _rigid.position,
                Interact = _isKeyPressed
            });
            _lastRecordedInput = _inputVec;

            Manager.Game.StartPlayer();

        }

        if (!_isPushing)
        {
            string dir = beforedic;
            if (_inputVec.x <= -0.5f) dir = "W";
            else if (_inputVec.x >= 0.5f) dir = "E";
            else if (_inputVec.y >= 0.5f) dir = "N";
            else if (_inputVec.y <= -0.5f) dir = "S";
            beforedic = dir;
        }
        string dirForAnim = _faceLocked ? _lockedDir : beforedic;

        string statePrefix = (_inputVec.magnitude > 0)
            ? "Walk"
            : "Idle";

        string target = $"{statePrefix}{dirForAnim}";
        var st = animator.GetCurrentAnimatorStateInfo(0);

        if (!st.IsName(target))
            animator.Play(target);


        if (_isPushing && _inputVec.sqrMagnitude == 0f)
        {
            animator.Play($"Idle{(_faceLocked ? _lockedDir : beforedic)}", 0, 0f);
        }
    }

    public void OnInteract(InputValue value)
    {
        _isKeyPressed = value.isPressed;

        if (!value.isPressed)
        {
            if (_box != null)
            {
                _canMoveBox = false;

                _boxRb = null;
                _faceLocked = false;
                var box = _box.GetComponent<BoxInteraction>();
                box.SetMovable(false);
                _box = null;
            }
        }
        if (_ending)
        {
            // 게임 매니져에서 마지막 씬으로 이동하게 하기
        }

        if (_isRecording)
        {
            _records.Add(new InputRecord
            {
                Time = Time.fixedTime - _recordStartTime,
                Input = _inputVec,
                Position = _rigid.position,
                Interact = _isKeyPressed
            });
        }
    }
    private void FixedUpdate()
    {
        _speed = _isPushing ? 3.5f : 7f;

        if (_isRecording && _inputVec != _lastRecordedInput)
        {
            _records.Add(new InputRecord
            {
                Time = Time.fixedTime - _recordStartTime,
                Input = _inputVec,
                Position = _rigid.position,
                Interact = _isKeyPressed
            });
            _lastRecordedInput = _inputVec;
            //Debug.Log($"[Player] 입력 기록: Time={Time.fixedTime}, Input={_inputVec}, Position={_rigid.position}");
        }
        Vector2 nextVec = _inputVec * _speed * Time.fixedDeltaTime;
        if (!_isPush)
            _rigid.MovePosition(_rigid.position + nextVec);

        if (_isPushing && _boxRb != null)
        {
            _boxRb.MovePosition(_boxRb.position + nextVec);
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
    }

    public void PushPlayer()
    {
        StartCoroutine(Push());
    }

    private IEnumerator Push()
    {
        _isPush = true;
        yield return new WaitForSeconds(0.5f);
        _isPush = false;
    }

    public void DiePlayer()
    {
        if (_isRecording)
        {
            _records.Add(new InputRecord
            {
                Time = Time.fixedTime - _recordStartTime,
                Input = Vector2.zero,
                Position = _rigid.position,
                Interact = false
            });
        }
        _isRecording = false;
        _inputVec = Vector2.zero;

        _dieCount++;
        Manager.Game.PlayerDieAndSave(_records, gameObject, _dieCount, _recordStartTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fire"))
        {
            _box = null;
        }
        if (other.CompareTag("Rocket"))
        {
            _ending = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (!other.CompareTag("Box")) return;

        var box = other.GetComponent<BoxInteraction>();
        if (box == null) return;

        if (_isKeyPressed && box != null && box.IsPlayerOnly)
        {
            _canMoveBox = true;
            _box = other.gameObject;
            _boxRb = other.attachedRigidbody;
            box.SetMovable(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Box")) return;

        var box = other.GetComponent<BoxInteraction>();
        if (box == null) return;

        if (_box != null && other.gameObject == _box)
        {
            _canMoveBox = false;
            _box = null;
            _boxRb = null;
            box.SetMovable(false);

            _faceLocked = false;
        }
    }

}
