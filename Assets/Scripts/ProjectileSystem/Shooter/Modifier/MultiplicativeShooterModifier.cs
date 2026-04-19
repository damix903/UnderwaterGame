using UnityEngine;

namespace ProjectileSystem
{
    [CreateAssetMenu(fileName = "SM_", menuName = "Data/Shooter/MulMod")]
    public class MultiplicativeShooterModifier : BaseShooterModifier
    {
        [SerializeField] private float recoil = 1f;
        [SerializeField] private float cooldown = 1f;
        [SerializeField] private float cost = 1f;

        public override void Apply(ref ShooterContext context)
        {
            context.recoil *= recoil == 0f ? 1f : recoil;
            context.cooldown *= cooldown == 0f ? 1f : cooldown;
            context.cost *= cost == 0f ? 1f : cost;
        }
    }
}