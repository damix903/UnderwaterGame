using System;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using VContainer;

public class Room : PoolableEntity, IEnemySpawnPointHolder
{
    [Header("Room Settings")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    
    [Space]
    [SerializeField] private int maxSpawnCount;
    [SerializeField] private List<EnemySpawnPoint> spawnPoints;
    
    public Transform StartPoint => startPoint;
    public Transform EndPoint => endPoint;
    public IReadOnlyList<EnemySpawnPoint> SpawnPoints => spawnPoints;
    public int MaxSpawnCount => maxSpawnCount;

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