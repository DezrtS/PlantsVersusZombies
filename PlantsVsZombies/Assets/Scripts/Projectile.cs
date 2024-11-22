using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public enum ProjectileType
    {
        Player,
        Enemy
    }

    [SerializeField] private ProjectileData projectileData;
    private Rigidbody2D rig;
    private ProjectileType projectileType;
    private float damage;
    private float lifetimeTimer = 0;

    public delegate void ProjectileHandler(Projectile projectile);
    public event ProjectileHandler OnProjectileHit;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    public void InitializeProjectile(ProjectileType projectileType, float damage)
    {
        this.projectileType = projectileType;
        this.damage = damage;
    }

    public void FireProjectile(Vector2 direction)
    {
        lifetimeTimer = 0;
        rig.AddForce(projectileData.Speed * direction, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        lifetimeTimer += Time.fixedDeltaTime;
        if (lifetimeTimer > projectileData.Lifetime)
        {
            lifetimeTimer = 0;
            OnProjectileHit?.Invoke(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && projectileType == ProjectileType.Player)
        {
            collision.transform.GetComponent<Zombie>().Damage(damage);
            OnProjectileHit?.Invoke(this);
        }
        else if (collision.CompareTag("Player") && projectileType == ProjectileType.Enemy)
        {
            collision.transform.GetComponent<CrazyDave>().Die();
            OnProjectileHit?.Invoke(this);
        }
    }

    public void ResetProjectile()
    {
        rig.linearVelocity = Vector2.zero;
    }
}
