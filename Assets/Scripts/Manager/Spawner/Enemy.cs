using MessagePipe;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class Enemy : PoolableEntity
{
    [Inject] private ISubscriber<ReleaseType> _subscriber;
    [Inject] private ICollisionConfig _collisionConfig;
    private AIController _controller;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _subscriber?.Subscribe((type) =>
        {
            if (type == ReleaseType.Enemy) Release();
        });
        
        if (data is EnemyData enemyData)
            GetComponent<AIController>().Initialize(enemyData);
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (!other.gameObject.TryGetComponent<IDamageable>(out var damageable)) return;
        
        if (damageable.TeamID == TeamID.Player)
            damageable.TakeDamage(new DamageInfo(gameObject, _collisionConfig.Damage, _collisionConfig.EffectData));
    }
}