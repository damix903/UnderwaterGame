using Movement;
using Movement.Data;
using UnityEngine;

namespace EnemyAI.Move
{
    [CreateAssetMenu(fileName = "MD_", menuName = "Data/Enemy/Move/Chase", order = 0)]
    public class ChaseMoveData : BaseMoveData
    {
        [SerializeField] private ChaseMovementModifier modifier;
        
        public override IMoveable CreateMove(CharacterMovement movement, Transform owner)
        {
            return new ChaseMove(movement, owner, modifier);
        }
    }
}