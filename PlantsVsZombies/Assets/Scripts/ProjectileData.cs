using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    public float Speed => speed;
    public float Lifetime => lifetime;
}
