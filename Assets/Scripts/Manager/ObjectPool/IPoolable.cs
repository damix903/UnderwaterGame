
using System;

public interface IPoolable
{
    public int DefaultCapacity { get; }
    public int MaxSize { get; }
    public void InitializePool(Action release);
}