using UnityEngine;
using VContainer;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private ObjectPoolManager poolManager;
    [SerializeField] private ProjectileDataBase bulletData;
    [SerializeField] private ProjectileData data;
    [SerializeField] private LayerMask detectionLayer;

    [Inject] private ProjectileSpawnManager _manager;
    
    public void Fire(Vector2 dir)
    {
        var obj = _manager.Spawn(bulletData, transform);
        
        obj.transform.position = transform.position;
        obj.transform.right = dir;
        obj.Initialize(bulletData, new ProjectileSpawnParams(gameObject, dir, detectionLayer, TeamID.Player), bulletData);
        // if (obj.TryGetComponent<ProjectileBase>(out var proj))
        // {
        //     proj.Initialize(bulletData, new ProjectileSpawnParams(gameObject, dir, detectionLayer, TeamID.Player), bulletData);
        // }
    }
}
