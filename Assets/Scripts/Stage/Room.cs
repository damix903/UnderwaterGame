using System;
using System.Collections.Generic;
using Manager;
using MessagePipe;
using Sensor;
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
    
    [Space]
    [SerializeField] private ColliderDetector clearPoint;
    
    public Transform StartPoint => startPoint;
    public Transform EndPoint => endPoint;
    public IReadOnlyList<EnemySpawnPoint> SpawnPoints => spawnPoints;
    public int MaxSpawnCount => maxSpawnCount;
    
    protected override ReleaseType ReleaseType => ReleaseType.Room;
    [Inject] private IPublisher<LevelClearedMessage> _levelClearedPub;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (clearPoint != null)
            clearPoint.OnTargetDetected += HandlePlayerDetected;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (clearPoint != null)
            clearPoint.OnTargetDetected -= HandlePlayerDetected;
    }

    private void HandlePlayerDetected(GameObject obj)
    {
        _levelClearedPub.Publish(new LevelClearedMessage());
    }
}