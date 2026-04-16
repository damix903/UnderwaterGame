using Movement;
using UnityEngine;

namespace EnemyAI.Move
{
    public interface IMoveable
    {
        public void Start(Vector2 position = default);
        public void MoveTick(Vector2 position = default);
        public void Stop();
    }

    public abstract class BaseMove : IMoveable
    {
        protected readonly CharacterMovement movement;

        public BaseMove(CharacterMovement movement)
        {
            this.movement = movement;
        }

        public virtual void Start(Vector2 position = default) { }
        public abstract void MoveTick(Vector2 position = default);
        public virtual void Stop() => movement.SetMovementInput(Vector2.zero);
    }
}