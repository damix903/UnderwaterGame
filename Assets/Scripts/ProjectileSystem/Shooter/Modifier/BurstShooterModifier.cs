using UnityEngine;

namespace ProjectileSystem
{
    [CreateAssetMenu(fileName = "SM_", menuName = "Data/Shooter/BurstMod")]
    public class BurstShooterModifier : BaseShooterModifier
    {
        [SerializeField] private int burstCount;
        [SerializeField] private float burstInterval;

        public override void Apply(ref ShooterContext context)
        {
            context.burstCount = burstCount;
            context.burstInterval = burstInterval;

            context.cost /= burstCount;
            context.recoil /= burstCount;
        }
    }
}