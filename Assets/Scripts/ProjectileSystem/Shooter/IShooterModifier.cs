using Utility;

namespace ProjectileSystem
{
    public interface IShooterModifier : ISortable
    {
        public void Apply(ref ShooterContext context);
    }

    public struct ShooterContext
    {
        public float recoil;
        public float cost;
        public float cooldown;
        public int burstCount;
        public float burstInterval;
        public int spreadCount;
        public float spreadAngle;

        public ShooterContext(float recoil, float cost, float cooldown)
        {
            this.recoil = recoil;
            this.cost = cost;
            this.cooldown = cooldown;
            burstCount = 1;
            burstInterval = 0f;
            spreadCount = 1;
            spreadAngle = 0f;
        }
    }
}