using Movement;
using UnityEngine;

namespace EnemyAI.Move
{
    public class StraightMove : BaseMove
    {
        private readonly Vector2 _moveDir;
    
        public StraightMove(CharacterMovement movement, Vector2 moveDir) : base(movement)
        {
            _moveDir = moveDir;
        }

        public override void MoveTick(Vector2 position = default)
        {
            movement?.SetMovementInput(_moveDir);
        }
    }
}