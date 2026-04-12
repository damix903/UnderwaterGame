using System.Collections.Generic;
using Utility;
using UnityEngine;

namespace Manager.Upgrade
{
    [CreateAssetMenu(fileName = "UpGradeList", menuName = "Data/UpGrade/List", order = 0)]
    public class UpgradeSetting : ScriptableObject
    {
        [SerializeField] private int upGradeCount = 3;
        [SerializeField] private List<UpgradeData> upGradeList;
        
        public List<UpgradeData> GetRandomUpGrade(IEnumerable<UpgradeData> exceptList)
        {
            return  Selection.RandomSelect(upGradeList, upGradeCount, exceptList);
        }
    }
}