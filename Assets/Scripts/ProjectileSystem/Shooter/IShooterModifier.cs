namespace ProjectileSystem
{
    public interface IShooterModifier
    {
        public void Apply(ShooterContext context);
    }

    public class ShooterContext
    {
        public float recoil;
        public float cost;
        public float cooldown;
        public int burstCount = 1;
        public float burstInterval = 0f;
        public int spreadCount = 1;
        public float spreadAngle = 0f;
    }
}