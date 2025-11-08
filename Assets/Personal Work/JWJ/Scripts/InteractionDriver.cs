using UnityEngine;

public class InteractionDriver : MonoBehaviour
{
    private IInteractor _source;
    private IInteractable _target;
    private IInteractable _current;
    private bool _prevKey;

    private void Awake()
    {
        _source = GetComponent<IInteractor>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var it) && it.CanInteract(_source))
            _target = it;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_target != null && other.TryGetComponent<IInteractable>(out var it) && it == _target)
            _target = null;

        if (_current != null && other.TryGetComponent<IInteractable>(out var cur) && cur == _current)
        {
            _current.EndInteract(_source);
            _current = null;
        }
    }

    private void FixedUpdate()
    {
        bool key = _source.IsKeyPressed;

        if (key && !_prevKey)
        {
            if (_current != null)
            {
                _current.EndInteract(_source);
                _current = null;
            }

            if (_target != null && _target.CanInteract(_source))
            {
                _target.BeginInteract(_source);
                _current = _target;
            }
        }

        if (_current != null && key)
            _current.UpdateInteract(_source);

        if (!key && _prevKey && _current != null)
        {
            _current.EndInteract(_source);
            _current = null;
        }

        _prevKey = key;
    }
}
