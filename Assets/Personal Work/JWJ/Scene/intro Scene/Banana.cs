using UnityEngine;

public class Banana : MonoBehaviour
{
    [SerializeField] private Dialogue _dialogue;

    public void DialogueTrigger()
    {
        _dialogue.StartTpying();
    }
}
