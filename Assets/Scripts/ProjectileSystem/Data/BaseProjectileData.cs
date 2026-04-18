using System;
using Manager;
using ProjectileSystem;
using SpawnSystem;
using UnityEngine;

public abstract class BaseProjectileData : EntityData, IProjectileBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float lifeTime;
    [SerializeField] private EffectData effectData;
    
    public float MaxSpeed => maxSpeed;
    public float Damage => damage;
    public float LifeTime => lifeTime;
    public EffectData EffectData => effectData;

    public abstract void OnUpdate(Rigidbody2D rb, Projectile proj);
}
