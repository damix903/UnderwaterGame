using System;
using UnityEngine;

public abstract class BaseProjectileData : EntityData, IProjectileBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float lifeTime;
    
    public float MaxSpeed => maxSpeed;
    public float Damage => damage;
    public float LifeTime => lifeTime;

    public abstract void OnUpdate(Rigidbody2D rb, Projectile proj);
}
