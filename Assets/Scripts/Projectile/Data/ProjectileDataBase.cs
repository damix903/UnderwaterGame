using System;
using UnityEngine;

public abstract class ProjectileDataBase : ScriptableObject, IProjectileBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float lifeTime;
    
    public float MaxSpeed => maxSpeed;
    public float Damage => damage;
    public float LifeTime => lifeTime;

    protected abstract Type ProjectileClass { get; }
    public abstract void OnUpdate(Rigidbody2D rb, ProjectileBase proj);
}
