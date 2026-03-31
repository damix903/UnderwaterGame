using UnityEngine;

public class EntityData : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    
    public GameObject Prefab => prefab;
}

[CreateAssetMenu(fileName = "EntityData", menuName = "Data/Entity/Projectile")]
public class ProjectileData : EntityData
{
    
}
