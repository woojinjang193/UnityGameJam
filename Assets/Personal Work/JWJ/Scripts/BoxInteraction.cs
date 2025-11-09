using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    [SerializeField] private bool _playerOnly = true;
    [SerializeField] private int _ownerId = 0;
    private Vector2 _spawnPos;
    public Vector2 SpawnPos => _spawnPos;

    private SpriteRenderer _sr;
    private Color _color;
    private Collider2D _col;
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _color = _sr.color;
        _col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        var allBoxes = FindObjectsByType<BoxInteraction>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        foreach (var box in allBoxes)
        {
            if (box == this) continue;

            var otherCol = box.GetComponent<Collider2D>();
            if (otherCol == null) continue;

            bool ignore = (box._ownerId != _ownerId);
            Physics2D.IgnoreCollision(_col, otherCol, ignore);
        }
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

        }
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
