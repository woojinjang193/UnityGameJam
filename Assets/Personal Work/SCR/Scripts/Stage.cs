using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private int stageNum;
    [SerializeField] public string stageName;
    [SerializeField] public Transform respawnPoint;
    [SerializeField] public bool IsBox;
    [SerializeField] public Transform[] BoxTransform;
}
