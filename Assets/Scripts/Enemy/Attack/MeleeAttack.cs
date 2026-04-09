using UnityEngine;

namespace Attack
{
    public class MeleeAttack : AttackBase<MeleeAttackData>
    {
        public MeleeAttack(Transform owner, EnemyBaseAttackData attackData, IAnimEventListenable listener) : base(owner, attackData, listener)
        {
        }
    }
}