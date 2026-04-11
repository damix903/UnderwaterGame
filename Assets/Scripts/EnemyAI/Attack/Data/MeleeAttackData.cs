using UnityEngine;

namespace Attack
{
    [CreateAssetMenu(fileName = "EAD", menuName = "Data/Enemy/Attack/Melee", order = 0)]
    public class MeleeAttackData : EnemyBaseAttackData
    {
        public override IAttackable2 CreateAttack(Transform parent, IAnimEventListenable listener)
        {
            return new MeleeAttack(parent, this, listener);
        }
    }
}