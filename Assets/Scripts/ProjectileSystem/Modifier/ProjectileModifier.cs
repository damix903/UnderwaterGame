using Manager.UpGrade;
using UnityEngine;

namespace ProjectileSystem
{
    public abstract class BaseProjectileModifier : BaseUpGradeEffect, IProjectileModifier
    {
        public abstract void OnSpawn(Projectile p);
        public abstract void OnHitToObstacle(Projectile p);
        public abstract void OnHitToDamageable(Projectile p, IDamageable damageable);

        public override void Apply(RunState state)
        {
            state.ProjModifiers.Add(this);
        }
    }
}