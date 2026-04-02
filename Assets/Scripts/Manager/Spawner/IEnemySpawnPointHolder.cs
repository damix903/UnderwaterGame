using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemySpawnPointHolder
{
    public IReadOnlyList<EnemySpawnPoint> SpawnPoints { get; }
    public int MaxSpawnCount { get; }
}

[System.Serializable]
public struct EnemySpawnPoint : IEquatable<EnemySpawnPoint>
{
    public Transform SpawnPoint;
    [Range(0f, 1f)] public float Probability;
    public List<EnemyType> AvailableTypes;

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