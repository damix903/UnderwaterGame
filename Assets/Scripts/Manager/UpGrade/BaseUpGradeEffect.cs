using UnityEngine;

namespace Manager.UpGrade
{
    [CreateAssetMenu(fileName = "UGE_", menuName = "Data/UpGrade/Effect", order = 0)]
    public abstract class BaseUpGradeEffect : ScriptableObject
    {
        public abstract void Apply(RunState state);
    }
}