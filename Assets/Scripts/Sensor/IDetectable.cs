using System;
using UnityEngine;

public interface IDetectable
{
    public event Action<GameObject> OnTargetDetected;
    public event Action<GameObject> OnTargetLost;
}