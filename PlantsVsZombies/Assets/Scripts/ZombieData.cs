using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Scriptable Objects/ZombieData")]
public class ZombieData : ScriptableObject
{
    public enum SanityEffect
    {
        None,
        Sunlust,
        Insanity,
        NotSoTough
    }

    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private SanityEffect effect;

    public float Health => health;
    public float Speed => speed;
    public SanityEffect Effect => effect;
}