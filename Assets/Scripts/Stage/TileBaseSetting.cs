using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;

namespace Stage
{
    [CreateAssetMenu(fileName = "TileBaseSetting", menuName = "Data/Setting/TileBase")]
    public class TileBaseSetting : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<TileBase, List<EnemyType>> enemySpawnTileBases;
        
        [SerializeField] private TileBase platformMarkerTileBase;
        [SerializeField] private TileBase platformTileBase;
        
        public TileBase PlatformMarkerTileBase => platformMarkerTileBase;
        public TileBase PlatformTileBase => platformTileBase;
        
        public IReadOnlyList<EnemyType> GetEnemyTypes(TileBase tileBase)
        {
            return enemySpawnTileBases.TryGetValue(tileBase, out var enemyTypes)
                ? enemyTypes : new List<EnemyType>();
        }
    }
}