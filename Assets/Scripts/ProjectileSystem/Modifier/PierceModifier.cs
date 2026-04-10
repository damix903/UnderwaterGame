using UnityEngine;

namespace ProjectileSystem
{
    [CreateAssetMenu(fileName = "PM", menuName = "Data/Projectile/PierceMod", order = 0)]
    public class PierceModifier : BaseProjectileModifier
    {
        [SerializeField] private int pierceCount = 3;
        
        public override void OnSpawn(Projectile p)
        {
            p.RunTimeStats.Durability = pierceCount;
        }

        public override void OnHitToObstacle(Projectile p)
        {
            p.RunTimeStats.Durability = 0;
        }

        public override void OnHitToDamageable(Projectile p, IDamageable damageable)
        {
            p.RunTimeStats.Durability -= 1;
        }
    }
}