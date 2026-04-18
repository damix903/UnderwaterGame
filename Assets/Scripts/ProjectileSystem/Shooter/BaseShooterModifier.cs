using Manager.Upgrade;
using UnityEngine;
using Utility;

namespace ProjectileSystem
{
    public abstract class BaseShooterModifier : BaseUpgradeEffect, IShooterModifier
    {
        [SerializeField] private int sortPriority;
        
        public abstract void Apply(ref ShooterContext context);

        public override void Apply(RunState state)
        {
            state.ShooterModifiers.Add(this);
            state.ShooterModifiers.SortByPriority();
        }

        public int SortPriority => sortPriority;
    }
}