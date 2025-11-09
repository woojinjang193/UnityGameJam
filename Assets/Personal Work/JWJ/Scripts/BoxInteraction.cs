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
                bool ignore = (a._ownerId != b._ownerId);
                Physics2D.IgnoreCollision(a._col, b._col, ignore);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var otherCol = collision.collider;

        // 박스끼리 충돌은 무시
        if (otherCol.GetComponent<BoxInteraction>() != null)
            return;

        if (_playerOnly)
        {
            if (!otherCol.TryGetComponent<PlayerController>(out _))
                Physics2D.IgnoreCollision(_col, otherCol, true);
        }
        else
        {
            if (!otherCol.TryGetComponent<EchoController>(out var echo) || echo.EchoID != _ownerId)
                Physics2D.IgnoreCollision(_col, otherCol, true);
        }
    }


}
