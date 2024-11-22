using UnityEngine;

public class MoveCommand : ICommand
{
    private Transform transform;
    private Vector2 movement;

    public MoveCommand(Transform transform, Vector2 movement)
    {
        this.transform = transform;
        this.movement = movement;
    }

    public void Execute()
    {
        transform.position += (Vector3)movement;
    }

    public void Undo()
    {
        transform.position -= (Vector3)movement;
    }
}