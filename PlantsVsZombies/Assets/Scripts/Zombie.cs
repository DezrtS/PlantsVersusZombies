using System;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] protected ZombieData zombieData;
    private float health;
    private float zombieMovementIncrease = 0;

    public ZombieData Data => zombieData;

    public delegate void ZombieHandler(Zombie zombie);
    public event ZombieHandler OnZombieDie;

    private void Start()
    {
        health = zombieData.Health;
        zombieMovementIncrease = GameManager.Instance.zombieMovementIncrease;
    }

    private void FixedUpdate()
    {
        OnUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            GameManager.Instance.EndGame();
        }
    }

    protected virtual void OnUpdate()
    {
        transform.Translate((zombieData.Speed + zombieMovementIncrease) * Time.fixedDeltaTime * Vector3.left);
    }

    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        OnZombieDie?.Invoke(this);
    }
}
