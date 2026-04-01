using System;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using VContainer;

public class Room : PoolableEntity
{
    [Header("Room Settings")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    
    [SerializeField] private List<EnemySpawnPoint> spawnPoints;
    
    public Transform StartPoint => startPoint;
    public Transform EndPoint => endPoint;
    public IReadOnlyList<EnemySpawnPoint> SpawnPoints => spawnPoints;
    
    [Inject] private ISubscriber<ReleaseType> _subscriber;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _subscriber?.Subscribe((type) =>
        {
            if (type == ReleaseType.Room) Release();
        });
    }
}

[System.Serializable]
public struct EnemySpawnPoint : IEquatable<EnemySpawnPoint>
{
    public Transform SpawnPoint;
    [Range(0f, 1f)] public float Probability;

    public bool Equals(EnemySpawnPoint other)
    {
        return Probability.Equals(other.Probability) && Equals(SpawnPoint, other.SpawnPoint);
    }

    public override bool Equals(object obj)
    {
        return obj is EnemySpawnPoint other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Probability, SpawnPoint);
    }
}