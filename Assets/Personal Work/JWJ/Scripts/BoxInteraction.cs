using Unity.VisualScripting;
using UnityEngine;

public class BoxInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _playerOnly = true;
    [SerializeField] private int _ownerId = 0;

    private Rigidbody2D _rigid;
    private Collider2D _col;

    private IInteractor _holder;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }
    public bool CanInteract(IInteractor who)
    {
        if (_playerOnly)
        {
            return who is MonoBehaviour mb && mb.CompareTag("Player");
        }
        else
        {
            return who.Id == _ownerId;
        }
    }
    public void BeginInteract(IInteractor who)
    {
        _holder = who;
    }
    public void EndInteract(IInteractor who)
    {
        if (_holder == who)
        {
            _holder = null;
        }
    }
    public void UpdateInteract(IInteractor who)
    {
        if (_holder == who)
            _rigid.MovePosition(_rigid.position + who.MoveDelta);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent<IInteractor>(out var src))
        {
            if (_playerOnly && !other.collider.CompareTag("Player"))
                Physics2D.IgnoreCollision(_col, other.collider, true);
            else if (!_playerOnly && src.Id != _ownerId)
                Physics2D.IgnoreCollision(_col, other.collider, true);
        }
    }
}
