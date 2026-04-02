using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Inject] private IEntityFactory<Enemy> _enemyFactory;
    
    private List<EnemySpawnPoint> _spawnPoints = new();
    private int _currentSpawnCount;
    
    private void Start()
    {
        
    }
    

    public void Spawn(List<EnemyData> enemies, IEnemySpawnPointHolder holder)
    {
        if (enemies == null || holder == null) return;
        
       _spawnPoints.Clear();
       _currentSpawnCount = 0;
       foreach (var p in holder.SpawnPoints)
           _spawnPoints.Add(p);

       while (_currentSpawnCount < holder.MaxSpawnCount && _spawnPoints.Count > 0)
       {
           var point = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
           if (GetEnemyData(enemies, point, out var data))
           {
               _enemyFactory.Create(data, point.SpawnPoint);
               _currentSpawnCount++;
               _spawnPoints.Remove(point);
           }
       }
    }

    private bool GetEnemyData(List<EnemyData> enemies, EnemySpawnPoint point, out EnemyData data)
    {
        data = enemies[Random.Range(0, enemies.Count)];
        while (!point.AvailableTypes.Contains(data.EnemyType))
        {
            data = enemies[Random.Range(0, enemies.Count)];
        }

        return true;
    }
}
