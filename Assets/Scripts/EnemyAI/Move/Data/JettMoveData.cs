using Movement;
using Movement.Data;
using UnityEngine;

namespace EnemyAI.Move
{
    [CreateAssetMenu(fileName = "MD_", menuName = "Data/Enemy/Move/Jett", order = 0)]
    public class JettMoveData : BaseMoveData
    {
        [SerializeField] private float speed;
        [SerializeField] private Vector2 dir;
        
        public override IMoveable CreateMove(CharacterMovement movement, Transform owner)
        {
            var listenable = owner.GetComponentInChildren<IAnimEventListenable>();
            if (listenable == null) Debug.LogError($"No IAnimEventListenable found in children of {owner.name}");

            return new JettMove(movement, listenable, speed, dir);
        }
    }
}