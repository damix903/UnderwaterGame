using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    [CreateAssetMenu(fileName = "SC_", menuName = "Data/Stage/Config", order = 0)]
    public class StageConfig : ScriptableObject
    {
        [Header("Room Data")]
        [SerializeField] private RoomData entranceRoom;
        [SerializeField] private RoomData exitRoom;
        [SerializeField] private List<RoomData> rooms;
        [Space] 
        [SerializeField] private int roomCountMin = 8;
        [SerializeField] private int roomCountMax = 12;
    
        [Header("Enemy Data")]
        [SerializeField] private List<EnemyData> enemies;
        
        [Header("Gimmick Data")]
        [SerializeField] private List<GimmickData> gimmicks;
    
        public RoomData EntranceRoom => entranceRoom;
        public RoomData ExitRoom => exitRoom;
        public List<RoomData> Rooms => rooms;
        public int RoomCount => Random.Range(roomCountMin, roomCountMax);
        public List<EnemyData> Enemies => enemies;
    }
}