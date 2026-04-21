using System.Collections.Generic;
using SpawnSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Stage
{
    [CreateAssetMenu(fileName = "RD_", menuName = "Data/Stage/Room")]
    public class RoomData : EntityData, IWeightable
    {
        [Header("Room Data")] 
        [SerializeField] private RoomType roomType;
        [SerializeField] [Range(0f, 1f)] private float difficulty;
        [SerializeField] private int weight = 10;
        [SerializeField] [Range(0f, 1f)] private float flipChance;
    
        [Space]
        [SerializeField] private List<EnemyData> additiveEnemies;
    
        public float Difficulty => difficulty;
        public int Weight => weight;
        public bool ShouldFlip => Random.value < flipChance;
        public IReadOnlyList<EnemyData>  AdditiveEnemies => additiveEnemies;
    }
    
    public enum RoomType {Normal, Resource, Gimmick}
}
