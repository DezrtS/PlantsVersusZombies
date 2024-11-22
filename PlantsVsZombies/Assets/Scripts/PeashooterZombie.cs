using UnityEngine;

public class PeashooterZombie : Zombie
{
    private PeashooterZombieData peashooterZombieData;
    private float shootTimer = 0;

    private void Awake()
    {
        peashooterZombieData = zombieData as PeashooterZombieData;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        shootTimer += Time.fixedDeltaTime;
        if (shootTimer > peashooterZombieData.AttackSpeed)
        {
            Shoot();
            shootTimer = 0;
        }
    }

    public void Shoot()
    {
        GameManager.Instance.SpawnAndShootProjectile(transform.position + Vector3.left, Vector2.left, Projectile.ProjectileType.Enemy, 999);
    }
}
