using Movement;
using UnityEngine;

namespace EnemyAI.Move
{
    public class ChaseMove : BaseMove
    {
        private readonly Transform _owner;
        private readonly IMovementModifier _modifier;
    
        public ChaseMove(CharacterMovement movement, Transform owner, IMovementModifier modifier) : base(movement)
        {
            _owner = owner;
            _modifier = modifier;
        }

        public override void Start(Vector2 position = default)
        {
            base.Start(position);
            movement.AddModifier(_modifier);
        }

        public override void MoveTick(Vector2 position = default)
        {
            if (_owner == null) return;
        
            var dir = (position - (Vector2)_owner.position).normalized;
            movement?.SetMovementInput(dir);
        }
    
        public override void Stop()
        {
            base.Stop();
            movement.RemoveModifier(_modifier);
        }
    }
}