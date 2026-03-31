using System;
using UnityEngine;

public abstract class EntitySpawnManager : MonoBehaviour
{
    [SerializeField] private SpawnStrategyType spawnStrategyType;
    [SerializeField] protected Transform[] spawnPoints;
    
    protected ISpawnStrategy spawnStrategy;
    
    protected enum SpawnStrategyType
    {
        Random
    }
    
    protected virtual void Awake()
    {
        spawnStrategy = new RandomSpawnStrategy(spawnPoints);
    }

    public abstract void Spawn();
}