using UnityEngine;

namespace Attack
{
    [CreateAssetMenu(fileName = "EAD", menuName = "Data/Enemy/Attack", order = 0)]
    public abstract class EnemyBaseAttackData : ScriptableObject
    {
        [SerializeField] private OverlayAnimData animData;
        [SerializeField] private float range;
        [SerializeField] private float cooldown;
        //[SerializeField] private float 
        
        public OverlayAnimData AnimData => animData;
        public float Range => range;
        public float Cooldown => cooldown;
        public abstract AttackType Type { get; }
    }
    
    public enum AttackType { Melee, Ranged }
}