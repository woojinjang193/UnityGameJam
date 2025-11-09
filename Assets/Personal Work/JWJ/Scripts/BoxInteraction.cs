using System.Collections;
using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    [SerializeField] private bool _playerOnly = true;
    [SerializeField] private int _ownerId = 0;

    private SpriteRenderer _sr;
    private Color _color;
    private Collider2D _col;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _color = _sr.color;
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }
    public void SetBox(bool isForPlayer, int ownerId)
    {
        if (isForPlayer)
        {
            _playerOnly = true;
            _ownerId = 0;
            _color.a = 1f;
            _sr.color = _color;
        }
        else
        {
            _playerOnly = false;
            _ownerId = ownerId;
            _color.a = 0.3f;
            _sr.color = _color;

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

        if (_playerOnly)
        {
            if (!otherCol.TryGetComponent<PlayerController>(out var player))
            {
                Physics2D.IgnoreCollision(_col, otherCol, true);
            }

        }
        else
        {
            var echo = otherCol.GetComponent<EchoController>();
            if (echo == null)
            {
                Physics2D.IgnoreCollision(_col, otherCol, true);
                return;
            }

            int echoId = echo.EchoID;

            if (echoId != _ownerId)
            {
                Physics2D.IgnoreCollision(_col, otherCol, true);
            }
        }
    }

}
