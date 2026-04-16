using Movement;
using UnityEngine;

namespace EnemyAI.Move
{
    public class PatrolMove : BaseMove
    {
        private readonly float _time;
    
        private Vector2 _moveDir;
        private float _timer;
    
        public PatrolMove(CharacterMovement movement, Vector2 dir, float time) : base(movement)
        {
            _moveDir = dir;
            _time = time;
            _timer = time;
        }

        public override void MoveTick(Vector2 position = default)
        {
            movement.SetMovementInput(_moveDir);
        
            _timer -= Time.fixedDeltaTime;
            if (_timer <= 0)
            {
                _moveDir *= -1;
                _timer = _time;
            }
        }
    }
}