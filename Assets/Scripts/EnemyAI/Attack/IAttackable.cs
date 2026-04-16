using UnityEngine;

namespace EnemyAI.Attack
{
    public interface IAttackable
    {
        public void StartAttack();
        public void CancelAttack();
        public bool CanAttack { get; }
        public bool IsCompleted { get; }
    }
}