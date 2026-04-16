using UnityEngine;

namespace EnemyAI.Attack
{
    public abstract class BaseAttackData : ScriptableObject
    {
        [SerializeField] private float range;
        [SerializeField] private float cooldown;

        public float Range => range;
        public float Cooldown => cooldown;

        public abstract IAttackable CreateAttack(ICharacterController owner, IAnimEventListenable listener);
    }
}