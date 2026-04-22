using System.Collections.Generic;
using Manager;
using MessagePipe;
using Sensor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
using VContainer;

namespace Stage
{
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
        
        [Space]
        public TileMapManager tileMapManager;
    
        public Transform StartPoint => startPoint;
        public Transform EndPoint => endPoint;
        public IReadOnlyList<EnemySpawnPoint> SpawnPoints => spawnPoints;
        public int MaxSpawnCount => maxSpawnCount;
        
        [SerializeField] private Tilemap spawnMarkerTilemap;
        [SerializeField] List<EnemySpawnPoint> bakedSpawnPoints = new List<EnemySpawnPoint>();
        [SerializeField] private TileBaseSetting tileBaseSetting; 

        [ContextMenu("Bake Spawn Points")]
        public void BakeSpawnPoints()
        {
            bakedSpawnPoints.Clear();

            var worldPosDict = spawnMarkerTilemap.GetTileDictionary<Vector3>();

            foreach (var pair in worldPosDict)
            {
                foreach (var pos in pair.Value)
                {
                    var spawnPoint = new EnemySpawnPoint
                    {
                        SpawnPoint = new GameObject($"SpawnPoint_{pos.x}_{pos.y}").transform,
                        Probability = 1f, // デフォルトの確率を設定
                        AvailableTypes = new List<EnemyType>() // デフォルトの敵タイプを設定
                    };
                    
                    spawnPoint.SpawnPoint.position = pos;
                    foreach (var type in tileBaseSetting.GetEnemyTypes(pair.Key))
                        spawnPoint.AvailableTypes.Add(type);
                    
                    bakedSpawnPoints.Add(spawnPoint);
                }
            }
            
            // foreach (var pos in spawnMarkerTilemap.cellBounds.allPositionsWithin)
            // {
            //     var tile = spawnMarkerTilemap.GetTile(pos);
            //     if (tile == null) continue;
            //     var worldPos = spawnMarkerTilemap.CellToWorld(pos);
            //     var spawnPoint = new EnemySpawnPoint
            //     {
            //         SpawnPoint = new GameObject($"SpawnPoint_{pos.x}_{pos.y}").transform,
            //         Probability = 1f, // デフォルトの確率を設定
            //         AvailableTypes = new List<EnemyType>() // デフォルトの敵タイプを設定
            //     };
            //     spawnPoint.SpawnPoint.position = worldPos;
            //     foreach (var type in tileBaseSetting.GetEnemyTypes(tile))
            //     {
            //         spawnPoint.AvailableTypes.Add(type);
            //     }
            //     bakedSpawnPoints.Add(spawnPoint);
            // }
        }
    
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
            
            tileMapManager.ClearTileMap();
        }

        private void HandlePlayerDetected(GameObject obj)
        {
            _levelClearedPub.Publish(new LevelClearedMessage());
        }
    }
}