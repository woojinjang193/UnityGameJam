using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    [SerializeField] private bool _playerOnly = true;
    [SerializeField] private int _ownerId = 0;

    public void SetBox(bool isForPlayer, int ownerId)
    {
        if (isForPlayer)
        {
            _playerOnly = true;
            _ownerId = 0;
        }
        else
        {
            _playerOnly = false;
            _ownerId = ownerId;
        }
    }
    
}
