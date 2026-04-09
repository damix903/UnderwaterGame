using UnityEngine;

namespace Attack
{
    public abstract class AttackBase<T> : IAttackable2 where T : EnemyBaseAttackData
    {
        protected readonly Transform owner;
        protected readonly T attackData;
        protected readonly IAnimEventListenable listener;
        
        private float _lastAttackTime;
        
        public AttackBase(Transform owner, EnemyBaseAttackData attackData, IAnimEventListenable listener)
        {
            this.owner = owner;
            this.listener = listener;

            if (attackData is T data)
            {
                this.attackData = data;
                _lastAttackTime = 0;
            }
            else
            {
                Debug.LogError($"Attack data type mismatch. Expected {typeof(T)}, got {attackData.GetType()}");
            }
        }
        
        public virtual void Attack(Transform target)
        {
            _lastAttackTime = Time.time;
        }

        public virtual bool CanAttack(Transform target)
        {
            if (target == null || owner == null) return false;
            if (Time.time - _lastAttackTime < attackData.Cooldown) return false;
            
            float distance = Vector3.Distance(owner.position, target.position);
            return distance <= attackData.Range;
        }

        public virtual bool IsCompleted => true;
    }
}