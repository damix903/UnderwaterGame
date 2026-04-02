using UnityEngine;

public interface IAttackable
{
    public void Attack(Vector2 direction = default);
    public bool CanAttack { get; }
}