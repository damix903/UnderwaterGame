using Movement;
using UnityEngine;

namespace EnemyAI.Attack
{
    [CreateAssetMenu(fileName = "AD_", menuName = "Data/Enemy/Attack/Charge")]
    public class ChargeAttackData : BaseAttackData
    {
        [SerializeField] private float speed;
        
        public float Speed => speed;
        
        public override IAttackable CreateAttack(ICharacterController owner, IAnimEventListenable listener)
        {
            var forceApplicable = owner.GameObject.GetComponent<IForceApplicable>();
            return new ChargeAttack(owner, this, listener, forceApplicable);
        }
    }
}