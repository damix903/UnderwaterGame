namespace ProjectileSystem
{
    public interface IProjectileModifier
    {
        public void OnSpawn(Projectile p);
        public void OnHitToObstacle(Projectile p);
        public void OnHitToDamageable(Projectile p, IDamageable damageable);
    }
}