using System;
using UnityEngine;

namespace Sensor
{
    public interface IDetectable
    {
        public event Action<GameObject> OnTargetDetected;
        public event Action<GameObject> OnTargetLost;
    }
}