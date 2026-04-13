using System;
using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using Utility.Lottery;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;


public class StageGenerator : MonoBehaviour, IStartable
{
    private IEntityFactory<Room> _factory;
    [SerializeField] private StageConfig stageConfig;
    [SerializeField] private int roomCount;

    public Transform StageStartPoint {get; private set;}
    public Transform StageEndPoint {get; private set;}
    private Transform _lastEndPoint;
    
    public event Action<Vector3> OnStageGenerated;
    
    [Inject] private IPublisher<ReleaseType> _publisher;
    [Inject] private EnemySpawner _spawner;

    [Inject]
    public void Construct(IEntityFactory<Room> factory)
    {
        _factory = factory;
    }

    public void Start()
    {
        //StartCoroutine(DelayGenerate(.5f));
    }

    [ContextMenu("Generate")]
    public void GenerateFromEditor()
    {
        _publisher?.Publish(ReleaseType.Room);
        _publisher?.Publish(ReleaseType.Enemy);
        Generate();
        //StartCoroutine(DelayGenerate());
    }

    private IEnumerator DelayGenerate(float delay = .1f)
    {
        yield return new WaitForSeconds(delay);
        Generate();
    }
    
    public void Generate()
    {
        var entrance = _factory.Create(stageConfig.EntranceRoom, transform);
        StageStartPoint = entrance.StartPoint;
        _lastEndPoint = entrance.EndPoint;
        
        int count = 0;
        var lastRoomData = stageConfig.EntranceRoom;
        while (count < roomCount)
        {
            var data = RandomSelector.SelectWithWeight(stageConfig.Rooms);
            //var data = stageConfig.Rooms[Random.Range(0, stageConfig.Rooms.Count)];
            if (data == lastRoomData) continue;

            GenerateRoom(data);
            lastRoomData = data;
            count++;
        }
        
        GenerateRoom(stageConfig.ExitRoom);
        StageEndPoint = _lastEndPoint;
        OnStageGenerated?.Invoke(StageStartPoint.position);
    }

    private void GenerateRoom(RoomData data)
    {
        var room = _factory.Create(data, _lastEndPoint);
        var amountToMove = _lastEndPoint.position - room.StartPoint.position;
        room.transform.position += amountToMove;
        
        bool shouldFlip = Random.Range(0f, 1f) < data.FlipChance;
        if (shouldFlip) room.transform.Rotate(0f, 180f, 0f);
        _lastEndPoint = room.EndPoint;
        
        var enemies = new List<EnemyData>(stageConfig.Enemies);
        foreach (var e in data.AdditiveEnemies)
            enemies.Add(e);
        
        _spawner.Spawn(enemies, room);
    }
}

public enum ReleaseType { Room, Enemy, Projectile, Item }