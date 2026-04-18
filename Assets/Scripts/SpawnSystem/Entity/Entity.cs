using System;
using SpawnSystem;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected EntityData data;

    public void Initialize(EntityData entityData)
    {
        data = entityData;
        OnInitialize();
    }

    protected virtual void OnInitialize() { }

    protected virtual void OnCollisionEnter2D(Collision2D other) { }

    protected virtual void Awake() { }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }
    protected virtual void OnDestroy() { }
}