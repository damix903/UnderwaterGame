using UnityEngine;

namespace Attack
{
    public abstract class AttackBase<T> : IAttackable2 where T : EnemyBaseAttackData
    {
        protected readonly Transform owner;
        protected readonly T attackData;
        protected readonly IAnimEventListenable listener;
        
        private float _lastAttackTime;
        protected bool IsInCoolDown => Time.time - _lastAttackTime < attackData.Cooldown;
        
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
        
        public void Attack(Transform target)
        {
            listener?.Register(AnimationEventType.FinishAnim, OnAnimFinished);
            IsCompleted = false;
            AttackInternal(target);
        }

        protected virtual void OnAnimFinished(bool obj)
        {
            IsCompleted = true;
            _lastAttackTime = Time.time;
        }

        public virtual bool CanAttack(Transform target)
        {
            if (target == null || owner == null) return false;
            if (IsInCoolDown) return false;
            
            float distance = Vector3.Distance(owner.position, target.position);
            return distance <= attackData.Range;
        }


        protected abstract void AttackInternal(Transform target);
        public virtual bool IsCompleted { get; protected set; }
    }
}