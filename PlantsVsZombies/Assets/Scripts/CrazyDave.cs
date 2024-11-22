using UnityEngine;

public class CrazyDave : MonoBehaviour
{
    private CommandInvoker commandInvoker;
    public int sanity = 0;
    public int damage = 4;

    private void Start()
    {
        commandInvoker = new CommandInvoker();
    }

    public void Move(Vector2 movement)
    {
        if (Random.Range(0, 100) < sanity)
        {
            if (Random.Range(0, 2) == 1)
            {
                commandInvoker.Undo();
            }
            else
            {
                commandInvoker.Redo();
            }
        }
        else
        {
            ICommand moveCommand = new MoveCommand(transform, movement);
            commandInvoker.Execute(moveCommand);
        }
    }

    public void Shoot()
    {
        GameManager.Instance.SpawnAndShootProjectile(transform.position + Vector3.right, Vector2.right, Projectile.ProjectileType.Player, damage);
    }

    public void Die()
    {
        GameManager.Instance.EndGame();
    }
}
