using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBase : MonoBehaviour, IPoolable
{
    private Rigidbody2D _rb;

    private ProjectileDataBase _projData;
    private ProjectileSpawnParams _spawnParams;

    private IProjectileBehaviour _behaviour;
    private Action _returnAction;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) _rb = gameObject.AddComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _behaviour?.OnUpdate(_rb, this);
    }

    public void Initialize(ProjectileDataBase data, ProjectileSpawnParams param, IProjectileBehaviour behaviour)
    {
        _projData = data;
        _behaviour = behaviour;
        _spawnParams = param;
        StartCoroutine(SetLifeTime(data.LifeTime));
    }

    private IEnumerator SetLifeTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _spawnParams.Owner || ((_spawnParams.DetectionLayer.value & (1 << other.gameObject.layer)) <= 0)) return;
        
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            if (damageable.TeamID != _spawnParams.OwnerTeamID)
            {
                OnHitToDamageable(damageable);
            }
        }
        
        OnHit();
    }

    public void InitializePool(Action returnAction) => _returnAction = returnAction;
    protected void ReturnToPool() => _returnAction?.Invoke();

    protected virtual void OnHitToDamageable(IDamageable damageable)
    {
        var info = new DamageInfo(
            _spawnParams.Owner,
            _projData.Damage,
            new EffectData()
        );
        damageable.TakeDamage(info);
    }

    protected virtual void OnHit()
    {
        ReturnToPool();
    }

    protected virtual void OnLifeTimeReached()
    {
        ReturnToPool();
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