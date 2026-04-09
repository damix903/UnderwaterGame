using UnityEngine;

namespace Attack
{
    [CreateAssetMenu(fileName = "EAD", menuName = "Data/Enemy/Attack/Melee", order = 0)]
    public abstract class MeleeAttackData : EnemyBaseAttackData
    {
        public override AttackType Type => AttackType.Melee;
    }
}