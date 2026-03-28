
using System;

public interface IPoolable
{
    void InitializePool(Action returnAction);
}