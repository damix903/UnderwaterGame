public interface IMovementModifier
{
    public void Apply(ref MovementRuntimeStats stats);
}