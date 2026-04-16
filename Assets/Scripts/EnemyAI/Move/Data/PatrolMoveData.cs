using Movement;
using UnityEngine;

namespace EnemyAI.Move
{
    [CreateAssetMenu(fileName = "MD_", menuName = "Data/Enemy/Move/Patrol", order = 0)]
    public class PatrolMoveData : BaseMoveData
    {
        [SerializeField] private Vector2 dir;
        [SerializeField] private bool isRandomDir;
        [SerializeField] private float flipTime;
        
        private Vector2 Dir => isRandomDir ? Random.insideUnitCircle.normalized : dir;
        
        public override IMoveable CreateMove(CharacterMovement movement, Transform owner)
        {
            return new PatrolMove(movement, Dir, flipTime);
        }
    }
}