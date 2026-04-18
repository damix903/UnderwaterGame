namespace ProjectileSystem
{
    public interface IProjectileModifier
    {
        public void OnSpawn(Projectile p);
        public void OnHitToObstacle(Projectile p);
        public void OnHitToDamageable(Projectile p, IDamageable damageable);
    }

    public class ProjectileRunTimeStats
    {
        public int Durability;
        public float LifeTime;
        public float Speed;
        public DamageInfo DamageInfo;

        public void Initialize(BaseProjectileData data, ProjectileSpawnParams param)
        {
            Durability = 0;
            LifeTime = data.LifeTime;
            Speed = data.MaxSpeed;
            DamageInfo = new DamageInfo(
                param.Owner,
                data.Damage,
                data.EffectData
            );
        }
    }
}