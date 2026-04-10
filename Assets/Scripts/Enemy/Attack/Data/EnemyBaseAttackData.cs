using UnityEngine;

namespace Attack
{
    public abstract class EnemyBaseAttackData : ScriptableObject
    {
        [SerializeField] private float range;
        [SerializeField] private float cooldown;
        [SerializeField] private float stopDuration;

        public float Range => range;
        public float Cooldown => cooldown;
        public float StopDuration => stopDuration;

        public abstract IAttackable2 CreateAttack(Transform parent, IAnimEventListenable listener);
    }
    
    public enum AttackType { Melee, Ranged }
}