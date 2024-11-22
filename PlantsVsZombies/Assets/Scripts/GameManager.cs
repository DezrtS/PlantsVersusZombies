using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] private CrazyDave crazyDave;
    [SerializeField] private TextMeshProUGUI moneyGUI;
    private int money = 0;

    [SerializeField] private GameObject projectile;
    [SerializeField] private ObjectPool projectilePool;
    [SerializeField] private int projectilePoolCount;
    [SerializeField] private Zombie[] zombiePrefabs;
    [SerializeField] private float zombieSpawnSpeed = 1;
    [SerializeField] private float zombieSpawnRateMultiplier = 0.01f;
    private float zombieSpawnTimer = 0;
    private int zombieCount = 0;

    [SerializeField] private Vector2 topRow;
    [SerializeField] private Vector2 bottomRow;
    [SerializeField] private int rowCount;
    private float rowOffset;

    public float zombieMovementIncrease = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
    }

    private void Start()
    {
        projectilePool.InitializePool(projectile, projectilePoolCount, true);
        rowOffset = (topRow.y - bottomRow.y) / (rowCount - 1);
        moneyGUI.text = $"Money: {money}";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && crazyDave.transform.position.y < topRow.y)
        {
            crazyDave.Move(new Vector2(0, rowOffset));
        }
        else if (Input.GetKeyDown(KeyCode.S) && crazyDave.transform.position.y > bottomRow.y)
        {
            crazyDave.Move(new Vector2(0, -rowOffset));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            crazyDave.Shoot();
        }
    }

    private void FixedUpdate()
    {
        zombieSpawnTimer += Time.fixedDeltaTime;
        if (zombieSpawnTimer > zombieSpawnSpeed - zombieSpawnRateMultiplier * Time.timeSinceLevelLoad)
        {
            SpawnZombie();
            zombieSpawnTimer = 0;
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SpawnZombie()
    {
        if (zombieCount < 100)
        {
            zombieCount++;
            int selectedZombie = Random.Range(0, zombiePrefabs.Length);
            Vector2 position = new Vector2(0, Random.Range(0, rowCount) * rowOffset) + bottomRow;
            Zombie newZombie = Instantiate(zombiePrefabs[selectedZombie], position, Quaternion.identity);
            newZombie.OnZombieDie += OnZombieDie;
        }
    }

    public void OnZombieDie(Zombie zombie)
    {
        switch (zombie.Data.Effect)
        {
            case ZombieData.SanityEffect.Sunlust:
                if (Random.Range(0, 2) == 1)
                {
                    SpawnZombie();
                    Debug.Log("SPAWNADDITIONAL");
                }
                money++;
                moneyGUI.text = $"Money: {money}";
                break;
            case ZombieData.SanityEffect.Insanity:
                crazyDave.sanity++;
                // You will need to shoot faster by pressing the space bar faster to stop the zombies and handle the random movements C:
                Debug.Log("SANITY");
                break;
            case ZombieData.SanityEffect.NotSoTough:
                crazyDave.damage++;
                zombieMovementIncrease += 0.25f;
                Debug.Log("NOTSOTOUGH");
                break;
        }

        zombie.OnZombieDie -= OnZombieDie;
        zombieCount--;
        Destroy(zombie.gameObject);
    }

    public void SpawnAndShootProjectile(Vector2 position, Vector2 direction, Projectile.ProjectileType projectileType, float damage)
    {
        GameObject newProjectile = projectilePool.GetObject();
        newProjectile.transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, -90 * direction.x));
        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.InitializeProjectile(projectileType, damage);
        projectile.FireProjectile(direction);
        projectile.OnProjectileHit += OnProjectileHit;
    }

    public void OnProjectileHit(Projectile projectile)
    {
        projectile.OnProjectileHit -= OnProjectileHit;
        projectile.ResetProjectile();
        projectilePool.ReturnToPool(projectile.gameObject);
    }
}
