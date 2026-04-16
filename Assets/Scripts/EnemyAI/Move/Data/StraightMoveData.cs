using Movement;
using UnityEngine;

namespace EnemyAI.Move
{
    [CreateAssetMenu(fileName = "MD_", menuName = "Data/Enemy/Move/Straight", order = 0)]
    public class StraightMoveData : BaseMoveData
    {
        [SerializeField] private Vector2 dir;
        [SerializeField] private bool isRandomDir;
        
        // insideUnitCircle は1の半径を持つ円の中のランダムな点を返すため、正規化して単位ベクトルにする必要がある
        private Vector2 Dir => isRandomDir ? Random.insideUnitCircle.normalized : dir;
        
        public override IMoveable CreateMove(CharacterMovement movement, Transform owner)
        {
            return new StraightMove(movement, Dir);
        }
    }
}