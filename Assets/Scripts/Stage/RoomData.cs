using System.Collections.Generic;
using SpawnSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Entity/Room")]
public class RoomData : EntityData, IWeightable
{
    [Header("Room Data")] 
    [SerializeField] private int weight = 10;
    [SerializeField] [Range(0f, 1f)] private float flipChance;
    
    [Space]
    [SerializeField] private List<EnemyData> additiveEnemies;
    
    public int Weight => weight;
    public float FlipChance => flipChance;
    public IReadOnlyList<EnemyData>  AdditiveEnemies => additiveEnemies;
}
