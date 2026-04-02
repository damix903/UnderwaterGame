using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : PoolableEntity
{
    private Rigidbody2D _rb;

    private BaseProjectileData _projData;
    private IProjectileBehaviour _behaviour;

    private Coroutine _lifeTimeCo;

    public ProjectileSpawnParams SpawnParams { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) _rb = gameObject.AddComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _behaviour?.OnUpdate(_rb, this);
    }

    public void Initialize(BaseProjectileData projData, ProjectileSpawnParams param, IProjectileBehaviour behaviour)
    {
        _projData = projData;
        _behaviour = behaviour;
        SpawnParams = param;
        
        StopLifeTime();
        _lifeTimeCo = StartCoroutine(SetLifeTime(projData.LifeTime));
    }

    private IEnumerator SetLifeTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        OnLifeTimeReached();
    }

    private void StopLifeTime()
    {
        if (_lifeTimeCo != null) StopCoroutine(_lifeTimeCo);
        _lifeTimeCo = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == SpawnParams.Owner || ((SpawnParams.DetectionLayer.value & (1 << other.gameObject.layer)) <= 0)) return;
        
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            if (damageable.TeamID != SpawnParams.OwnerTeamID)
                OnHitToDamageable(damageable);
        }
        
        OnHit();
    }

    private void OnHitToDamageable(IDamageable damageable)
    {
        var info = new DamageInfo(
            SpawnParams.Owner,
            _projData.Damage,
            new EffectData()
        );
        damageable.TakeDamage(info);
    }

    private void OnHit()
    {
        Release();
        StopLifeTime();
    }

    private void OnLifeTimeReached()
    {
        Release();
    }
}

public struct ProjectileSpawnParams
{
    public readonly GameObject Owner;
    public readonly Vector2 Dir;
    public readonly LayerMask DetectionLayer;
    public readonly TeamID OwnerTeamID;

    public ProjectileSpawnParams(GameObject owner, Vector2 dir, LayerMask detectionLayer, TeamID ownerTeamID)
    {
        Owner = owner;
        Dir = dir;
        DetectionLayer = detectionLayer;
        OwnerTeamID = ownerTeamID;
    }
}