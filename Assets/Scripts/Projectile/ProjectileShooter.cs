using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private ObjectPoolManager poolManager;
    [SerializeField] private ProjectileDataBase bulletData;
    [SerializeField] private LayerMask detectionLayer;

    public void Fire(Vector2 dir)
    {
        var obj = poolManager.Get(PoolType.Projectile);

        obj.transform.position = transform.position;
        obj.transform.right = dir;
        if (obj.TryGetComponent<ProjectileBase>(out var proj))
        {
            proj.Initialize(bulletData, new ProjectileSpawnParams(gameObject, dir, detectionLayer, TeamID.Player), bulletData);
        }
    }
}
