public interface IMovementModifier
{
    MovementRuntimeStats Apply(MovementRuntimeStats stats);
}