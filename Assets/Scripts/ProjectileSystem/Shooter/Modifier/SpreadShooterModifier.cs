using UnityEngine;

namespace ProjectileSystem
{
    [CreateAssetMenu(fileName = "SM_", menuName = "Data/Shooter/SpreadMod")]
    public class SpreadShooterModifier : BaseShooterModifier
    {
        [SerializeField] private int spreadCount;
        [SerializeField] private float spreadAngle;

        public override void Apply(ref ShooterContext context)
        {
            context.spreadCount = spreadCount;
            context.spreadAngle = spreadAngle;
        }
    }
}