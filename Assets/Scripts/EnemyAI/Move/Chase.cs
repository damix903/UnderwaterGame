using UnityEngine;

public class Chase : BaseMove
{
    private readonly Transform _owner;
    
    public Chase(CharacterMovement movement, Transform owner) : base(movement)
    {
        _owner = owner;
    }

    public override void Move(Vector2 position = default)
    {
        if (_owner == null) return;
        
        var dir = (position - (Vector2)_owner.position).normalized;
        movement?.SetMovementInput(dir);
    }
}