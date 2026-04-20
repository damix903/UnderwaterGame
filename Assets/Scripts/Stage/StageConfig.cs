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
    
        [Header("Enemy Data")]
        [SerializeField] private List<EnemyData> enemies;
        
        [Header("Gimmick Data")]
        [SerializeField] private List<GimmickData> gimmicks;
    
        public RoomData EntranceRoom => entranceRoom;
        public RoomData ExitRoom => exitRoom;
        public List<RoomData> Rooms => rooms;
        public List<EnemyData> Enemies => enemies;
    }
}