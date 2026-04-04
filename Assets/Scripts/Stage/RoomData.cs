using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Entity/Room")]
public class RoomData : EntityData
{
    [SerializeField] private List<EnemyData> additiveEnemies;
    
    public IReadOnlyList<EnemyData>  AdditiveEnemies => additiveEnemies;
}
