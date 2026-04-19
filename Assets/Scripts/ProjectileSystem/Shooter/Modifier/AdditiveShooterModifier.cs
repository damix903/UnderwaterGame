using UnityEngine;

namespace ProjectileSystem
{
    [CreateAssetMenu(fileName = "SM_", menuName = "Data/Shooter/AddMod")]
    public class AdditiveShooterModifier : BaseShooterModifier
    {
        [SerializeField] private float recoil;
        [SerializeField] private float cooldown;
        [SerializeField] private float cost;

        public override void Apply(ref ShooterContext context)
        {
            context.recoil += recoil;
            context.cooldown += cooldown;
            context.cost += cost;
        }
    }
}