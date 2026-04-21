using Movement;
using UnityEngine;

namespace EnemyAI.Attack
{
    public class ChargeAttack : AttackBase<ChargeAttackData>
    {
        private readonly IForceApplicable _forceApplicable;
        
        public ChargeAttack(ICharacterController owner, ChargeAttackData data, IAnimEventListenable listenable, IForceApplicable forceApplicable)
            : base(owner, data, listenable)
        {
            _forceApplicable = forceApplicable;
        }

        protected override void StartAttackInternal()
        {
            listenable?.Register(AnimationEventType.MoveTrigger, OnMove);
        }

        private void OnMove(bool isStarted)
        {
            if (owner == null || owner.Target == null) return;
            var dir = owner.Target.position - owner.GameObject.transform.position;
            var dir2D = new Vector2(dir.x, dir.y).normalized;
            _forceApplicable.AddImpulseForce(dir2D * data.Speed);
        }
        
        protected override void CancelAttackInternal()
        {
            listenable?.Remove(AnimationEventType.MoveTrigger, OnMove);
        }
    }
}