using UnityEngine;

namespace EnemyAI.Attack
{
    public abstract class AttackBase<T> : IAttackable where T : BaseAttackData
    {
        protected readonly ICharacterController owner;
        protected readonly T data;
        protected readonly IAnimEventListenable listenable;
        
        private float _lastAttackTime;
        protected bool IsInCoolDown => Time.time - _lastAttackTime < data.Cooldown;
        protected bool IsTargetAvailable => owner != null && owner.Target != null;
        protected bool IsInAttackRange
        {
            get
            {
                if (!IsTargetAvailable) return false;
                float distance = Vector3.Distance(owner.GameObject.transform.position, owner.Target.position);
                return distance <= data.Range;
            }
        }

        public virtual bool CanAttack
        {
            get
            {
                if (!IsTargetAvailable) return false;
                
                return !IsInCoolDown && IsInAttackRange;
            }
        }

        public virtual bool IsCompleted { get; protected set; }
        
        public AttackBase(ICharacterController owner, T data, IAnimEventListenable listenable)
        {
            this.owner = owner;
            this.listenable = listenable;
            this.data = data;
            _lastAttackTime = 0;
        }
        
        public void StartAttack()
        {
            listenable?.Register(AnimationEventType.FinishAnim, OnAnimFinished);
            IsCompleted = false;
            StartAttackInternal();
        }
        
        public void CancelAttack()
        {
            Cleanup();
            CancelAttackInternal();
        }
        
        private void OnAnimFinished(bool isStarted)
        {
            if (!isStarted) return;
            Cleanup();
            OnAnimFinished();
        }

        protected abstract void StartAttackInternal();
        protected virtual void CancelAttackInternal() {}
        protected virtual void OnAnimFinished() {}

        protected void Cleanup()
        {
            IsCompleted = true;
            _lastAttackTime = Time.time;
            listenable?.Remove(AnimationEventType.FinishAnim, OnAnimFinished);
        }
    }
}