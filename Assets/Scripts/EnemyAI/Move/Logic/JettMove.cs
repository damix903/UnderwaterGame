using Movement;
using UnityEngine;

namespace EnemyAI.Move
{
    public class JettMove : BaseMove
    {
        private readonly IAnimEventListenable _listenable;
        private readonly float _speed;
        private Vector2 _dir;
        
        public JettMove(CharacterMovement movement, IAnimEventListenable listenable, float speed, Vector2 dir) : base(movement)
        {
            _listenable = listenable;
            _speed = speed;
            _dir = dir.normalized;
        }

        public override void Start(Vector2 position = default)
        {
            base.Start(position);
            _listenable?.Register(AnimationEventType.MoveTrigger, OnMove);
        }

        private void OnMove(bool isStarted)
        {
            if (!isStarted) return;
            
            movement?.AddImpulseForce(_dir * _speed);
            _dir.x *= -1;
        }

        public override void MoveTick(Vector2 position = default) { }
        
        public override void Stop()
        {
            base.Stop();
            _listenable?.Remove(AnimationEventType.MoveTrigger, OnMove);
        }
    }
}