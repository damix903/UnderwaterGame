using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UI;
using UnityEngine;
using Utility.Lottery;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;


public class StageGenerator : MonoBehaviour
{
    private IEntityFactory<Room> _factory;
    [SerializeField] private StageConfig stageConfig;
    [SerializeField] private int roomCount;

    private Transform _lastEndPoint;
    
    [Inject] private IPublisher<ReleaseType> _publisher;
    [Inject] private EnemySpawner _spawner;

    [Inject]
    public void Construct(IEntityFactory<Room> factory)
    {
        _factory = factory;
    }
    
    [Inject] private UpgradePresenter _upgradePresenter;
    [ContextMenu("StartUpGrade")]
    public void StartUpGradePhase()
    {
        _upgradePresenter.StartUpgradeSelectionAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    [ContextMenu("Generate")]
    public Vector3 Generate()
    {
        _publisher?.Publish(ReleaseType.Room);
        _publisher?.Publish(ReleaseType.Enemy);
        return ProcessGenerate();
    }
    
    private Vector3 ProcessGenerate()
    {
        var entrance = _factory.Create(stageConfig.EntranceRoom, transform);
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
        
        return entrance.StartPoint.position;
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