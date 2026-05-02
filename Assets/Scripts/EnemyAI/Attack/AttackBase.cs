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

                float distance = Vector3.Distance(owner.Transform.position, owner.Target.position);
                return distance <= data.Range;
            }
        }

        // デフォルトではクールダウン中ではないかつ攻撃範囲にターゲットがいること
        public virtual bool CanAttack => !IsInCoolDown && IsInAttackRange;

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
            OnAttackStarted();
        }
        
        public void CancelAttack()
        {
            Cleanup();
            OnAttackCancelled();
        }
        
        private void OnAnimFinished(bool isStarted)
        {
            if (!isStarted) return;
            Cleanup();
            OnAnimFinished();
        }

        protected abstract void OnAttackStarted();
        protected virtual void OnAttackCancelled() {}
        protected virtual void OnAnimFinished() {}

        protected void Cleanup()
        {
            IsCompleted = true;
            _lastAttackTime = Time.time;
            listenable?.Remove(AnimationEventType.FinishAnim, OnAnimFinished);
        }
    }
}