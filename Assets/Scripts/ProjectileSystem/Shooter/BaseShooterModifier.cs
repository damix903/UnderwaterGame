using Manager.Upgrade;
using UnityEngine;

namespace ProjectileSystem
{
    public abstract class BaseShooterModifier : BaseUpgradeEffect, IShooterModifier
    {
        public abstract void Apply(ShooterContext context);

        public override void Apply(RunState state)
        {
            state.ShooterModifiers.Add(this);
        }
    }

    public class RecoilShooterModifier : BaseShooterModifier
    {
        [SerializeField] private float _recoil;

        public override void Apply(ShooterContext context)
        {
            context.recoil += _recoil;
        }
    }
}