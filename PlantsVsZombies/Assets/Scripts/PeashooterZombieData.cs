using UnityEngine;

[CreateAssetMenu(fileName = "PeashooterZombieData", menuName = "Scriptable Objects/PeashooterZombieData")]
public class PeashooterZombieData : ZombieData
{
    [SerializeField] private float attackSpeed;

    public float AttackSpeed => attackSpeed;
}
