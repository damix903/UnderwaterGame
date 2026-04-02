using System;
using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;


public class StatgeGenerator : MonoBehaviour
{
    private IEntityFactory<Room> _factory;
    [SerializeField] private StageConfig stageConfig;
    [SerializeField] private int roomCount;

    public Transform StageStartPoint {get; private set;}
    public Transform StageEndPoint {get; private set;}
    private Transform _lastEndPoint;
    
    [Inject] private IPublisher<ReleaseType> _publisher;
    [Inject] private EnemySpawner _spawner;

    [Inject]
    public void Construct(IEntityFactory<Room> factory)
    {
        _factory = factory;
    }

    private void Start()
    {
        Generate();
    }

    [ContextMenu("Generate")]
    public void GenerateFromEditor()
    {
        _publisher?.Publish(ReleaseType.Room);
        _publisher?.Publish(ReleaseType.Enemy);
        StartCoroutine(DelayGenerate());
    }

    private IEnumerator DelayGenerate()
    {
        yield return new WaitForSeconds(.1f);
        Generate();
    }
    
    public void Generate()
    {
        var entrance = _factory.Create(stageConfig.EntranceRoom, transform);
        StageStartPoint = entrance.StartPoint;
        
        int count = 0;
        _lastEndPoint = entrance.EndPoint;
        while (count < roomCount)
        {
            var data = stageConfig.Rooms[Random.Range(0, stageConfig.Rooms.Count)];

            GenerateRoom(data);
            count++;
        }
        
        GenerateRoom(stageConfig.ExitRoom);
        StageEndPoint = _lastEndPoint;
    }

    private void GenerateRoom(RoomData data)
    {
        var room = _factory.Create(data, _lastEndPoint);
        var amountToMove = _lastEndPoint.position - room.StartPoint.position;
        room.transform.position += amountToMove;
        _lastEndPoint = room.EndPoint;
        
        var enemies = stageConfig.Enemies;
        foreach (var e in data.AdditiveEnemies)
        {
            enemies.Add(e);
        }
        
        _spawner.Spawn(enemies, room);
    }

    private List<EnemySpawnPoint> _points = new();
}

public enum ReleaseType { Room, Enemy}