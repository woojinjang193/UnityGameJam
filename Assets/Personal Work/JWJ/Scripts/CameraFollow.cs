using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Camera _카메라;
    [SerializeField] private Transform _target;
    private void Awake()
    {
        _카메라 = GetComponent<Camera>();

    }
    private void Start()
    {
        _target = FindFirstObjectByType<PlayerController>().gameObject.transform;
    }
    void Update()
    {
        if(_target != null)
        {
            _카메라.transform.position = new Vector3(_target.position.x, _target.position.y, -10);
        }
    }
}
