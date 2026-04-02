using System;
using System.Collections;
using UnityEngine;
using VContainer;

public class ProjectileSpawnManager : MonoBehaviour
{
    [SerializeField] private ObjectPoolManager _objectPoolManager;
    private IEntityFactory<Projectile>  _factory;

    private void Awake()
    {
        //_factory = new PoolableEntityFactory<ProjectileBase>(_objectPoolManager);
    }

    [Inject]
    public void Construct(IEntityFactory<Projectile> factory)
    {
        _factory = factory;
    }

    public Projectile Spawn(EntityData data, Transform spawnPoint)
    {
        var proj = _factory.Create(data, spawnPoint);
        Debug.Log(proj);
        return proj;
    }
}