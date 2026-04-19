using Movement;
using Movement.Data;
using UnityEngine;

namespace EnemyAI.Move
{
    [CreateAssetMenu(fileName = "MD_", menuName = "Data/Enemy/Move/Strafe", order = 0)]
    public class StrafeMoveData : BaseMoveData
    {
        [SerializeField] private float directionChangeInterval = 2f;
        [SerializeField] private float distanceToMaintain = 3f;
        [SerializeField] private float distanceThreshold = 0.5f;
        
        public float DirectionChangeInterval => directionChangeInterval;
        public float DistanceToMaintain => distanceToMaintain;
        public float DistanceThreshold => distanceThreshold;
        
        public override IMoveable CreateMove(CharacterMovement movement, Transform owner)
        {
            return new StrafeMove(movement, owner, this);
        }
    }
}