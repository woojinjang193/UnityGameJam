using System.Collections;
using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    [SerializeField] private bool _playerOnly = true;
    [SerializeField] private int _ownerId = 0;
    private Vector2 _spawnPos;
    public Vector2 SpawnPos => _spawnPos;

    private SpriteRenderer _sr;
    private Color _color;

    private Rigidbody2D _rb;

    public bool IsPlayerOnly => _playerOnly;
    public int OwnerId => _ownerId;

    [SerializeField] private Collider2D _solidCol; // 물리 충돌용
    [SerializeField] private Collider2D _triggerCol;
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _color = _sr.color;
        _rb = GetComponent<Rigidbody2D>();
        if (_solidCol == null)
        {
            var cols = GetComponents<Collider2D>();
            _solidCol = System.Array.Find(cols, c => !c.isTrigger);
        }
    }
    public void SetMovable(bool canMove)
    {
        _rb.bodyType = canMove ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
    }

    public void SetBox(bool isForPlayer, int ownerId, Vector2 spawnPos)
    {
        if (isForPlayer)
        {
            _playerOnly = true;
            _ownerId = 0;
            _color.a = 1f;
            _sr.color = _color;
            _spawnPos = spawnPos;
        }
        else
        {
            _playerOnly = false;
            _ownerId = ownerId;
            _color.a = 0.3f;
            _sr.color = _color;
            _spawnPos = spawnPos;

            var renderers = GetComponentsInChildren<SpriteRenderer>(true);
            if (renderers != null)
            {
                foreach (var renderer in renderers)
                {
                    var c = renderer.color;
                    c.a = 0.3f;
                    renderer.color = c;
                }
            }
            
        }
    }
    public static void InitCollisionAll()
    {
        var allBoxes = FindObjectsByType<BoxInteraction>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        foreach (var a in allBoxes)
        {
            foreach (var b in allBoxes)
            {
                if (a == b) continue;
                if (!a._solidCol || !b._solidCol) continue;
                bool ignore = (a._ownerId != b._ownerId);
                Physics2D.IgnoreCollision(a._solidCol, b._solidCol, ignore);
            }
        }
    }


    public void PushBox()
    {
        StartCoroutine(Push());
    }

    private IEnumerator Push()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(0.5f);
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var otherCol = collision.collider;

        if (otherCol.GetComponent<BoxInteraction>() != null)
            return;

        if (_playerOnly)
        {
            if (!otherCol.TryGetComponent<PlayerController>(out var player))
            {
                Physics2D.IgnoreCollision(_solidCol, otherCol, true);
            }
        }
        else
        {
            var echo = otherCol.GetComponent<EchoController>();
            if (echo == null)
            {
                Physics2D.IgnoreCollision(_solidCol, otherCol, true);
                return;
            }

            int echoId = echo.EchoID;
            if (echoId != _ownerId)
            {
                Physics2D.IgnoreCollision(_solidCol, otherCol, true);
            }
        }
    }
}
