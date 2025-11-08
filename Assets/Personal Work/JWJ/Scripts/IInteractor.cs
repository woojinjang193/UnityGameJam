using UnityEngine;

public interface IInteractor
{
    int Id { get; }
    bool IsKeyPressed { get; }
    Vector2 MoveDelta { get; }
}
