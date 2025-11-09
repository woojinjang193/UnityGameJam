using UnityEngine;

public class StartDialogueEvent : MonoBehaviour
{
    [SerializeField] private EndingDialogue _dialogue;

    public void DialogueStart()
    {
        _dialogue.StartTpying();
        gameObject.SetActive(false);
    }
}
