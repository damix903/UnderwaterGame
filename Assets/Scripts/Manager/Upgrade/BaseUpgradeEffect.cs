using System;
using UnityEngine;

namespace Manager.Upgrade
{
    public abstract class BaseUpgradeEffect : ScriptableObject
    {
        public abstract void Apply(RunState state);
    }
}