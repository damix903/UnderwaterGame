using UnityEngine;

namespace Manager.Upgrade
{
    [CreateAssetMenu(fileName = "UGE_", menuName = "Data/Upgrade/Effect", order = 0)]
    public abstract class BaseUpgradeEffect : ScriptableObject
    {
        public abstract void Apply(RunState state);
    }
}