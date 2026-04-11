using UnityEngine;

namespace Attack
{
    public interface IAttackable2
    {
        public void Attack(Transform target);
        public bool CanAttack(Transform target);
        public bool IsCompleted { get; }
    }
}