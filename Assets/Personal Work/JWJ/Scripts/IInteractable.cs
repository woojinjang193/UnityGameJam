using UnityEngine;

public interface IInteractable
{
    bool CanInteract(IInteractor who);
    void BeginInteract(IInteractor who);
    void EndInteract(IInteractor who);
    void UpdateInteract(IInteractor who);
}
