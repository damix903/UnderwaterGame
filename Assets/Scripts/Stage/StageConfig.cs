using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SC", menuName = "Data/Stage", order = 0)]
public class StageConfig : ScriptableObject
{
    [Header("Room Data")]
    [SerializeField] private RoomData entranceRoom;
    [SerializeField] private RoomData exitRoom;
    [SerializeField] private List<RoomData> rooms;
    
    [Header("Enemy Data")]
    [SerializeField] private List<EnemyData> enemies;
    //[SerializeField] private 
    
    public RoomData EntranceRoom => entranceRoom;
    public RoomData ExitRoom => exitRoom;
    public List<RoomData> Rooms => rooms;
    public List<EnemyData> Enemies => enemies;
}