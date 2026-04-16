using Movement;
using UnityEngine;

namespace EnemyAI.Move
{
    public abstract class BaseMoveData : ScriptableObject
    {
        public abstract IMoveable CreateMove(CharacterMovement movement, Transform owner);
    }
}