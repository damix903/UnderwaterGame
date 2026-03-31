using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSpawnStrategy : ISpawnStrategy
{
    private List<Transform> _unusedSpawnPoints;
    private readonly Transform[] _spawnPoints;

    public RandomSpawnStrategy(Transform[] spawnPoints)
    {
        _spawnPoints = spawnPoints;
        _unusedSpawnPoints = new List<Transform>(_spawnPoints);
    }
    
    public Transform GetSpawnPoint()
    {
        if (!_unusedSpawnPoints.Any())
        {
            _unusedSpawnPoints = new List<Transform>(_spawnPoints);
        }
        
        var randomIndex = Random.Range(0, _unusedSpawnPoints.Count);
        var randomPoint = _unusedSpawnPoints[randomIndex];
        _unusedSpawnPoints.RemoveAt(randomIndex);
        
        return randomPoint;
    }
}