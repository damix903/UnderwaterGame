using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MessagePipe;
using SpawnSystem;
using UI;
using UnityEngine;
using Utility.Lottery;
using VContainer;
using Random = UnityEngine.Random;

namespace Stage
{
    public class StageGenerator : MonoBehaviour
    {
        private IEntityFactory<Room> _factory;
        [SerializeField] private StageConfig stageConfig;
        
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
            var entrance = _factory.Create(stageConfig.EntranceRoom, new SpawnPoint(transform.position));
            _lastEndPoint = entrance.EndPoint;
        
            GenerateMiddleRooms();

            GenerateRoom(stageConfig.ExitRoom);
        
            return entrance.StartPoint.position;
        }

        private void GenerateMiddleRooms()
        {
            int count = 0;
            int roomCount = stageConfig.RoomCount;
            var lastRoomData = stageConfig.EntranceRoom;
            
            while (count < roomCount)
            {
                var data = RandomSelector.SelectWithWeight(stageConfig.Rooms);
                if (data == lastRoomData) continue;

                GenerateRoom(data);
                lastRoomData = data;
                count++;
            }
        }

        private void GenerateRoom(RoomData data)
        {
            var room = _factory.Create(data, new SpawnPoint());
            var amountToMove = _lastEndPoint.position - room.StartPoint.position;
            room.transform.position += amountToMove;
            
            if (data.ShouldFlip) room.transform.Rotate(0f, 180f, 0f);
            _lastEndPoint = room.EndPoint;

            var platformPos = room.tileMapManager.PlatformPos;
            foreach (var pos in platformPos)
                if (Random.value < 0.5f) room.tileMapManager.SetPlatform(pos, Random.Range(3, 6));
            
            var enemies = new List<EnemyData>(stageConfig.Enemies);
            foreach (var e in data.AdditiveEnemies)
                enemies.Add(e);
        
            _spawner.Spawn(enemies, room);
        }
    }

    public enum ReleaseType { Room, Enemy, Projectile, Item, Audio }
}