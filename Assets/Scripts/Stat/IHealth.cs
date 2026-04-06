using System;

namespace Stat
{
    public interface IHealth
    {
        public float CurrentHealth { get;}
        public float MaxHealth { get;}
        public void ChangeHealth(float amount);
        public event Action<HealthChangeEventArgs> OnHealthChanged;
    }
}

public struct HealthChangeEventArgs
{
    public readonly float Current;
    public readonly float Max;
    public readonly float ChangedAmount;

    public HealthChangeEventArgs(float current, float max, float changedAmount)
    {
        Current = current;
        Max = max;
        ChangedAmount = changedAmount;
    }
}