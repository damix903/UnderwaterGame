using System.Collections.Generic;
using Utility;
using UnityEngine;

namespace Manager.UpGrade
{
    [CreateAssetMenu(fileName = "UpGradeList", menuName = "Data/UpGrade/List", order = 0)]
    public class UpGradeSetting : ScriptableObject
    {
        [SerializeField] private int upGradeCount = 3;
        [SerializeField] private List<UpGradeData> upGradeList;
        
        public List<UpGradeData> GetRandomUpGrade(IEnumerable<UpGradeData> exceptList)
        {
            return  Selection.RandomSelect(upGradeList, upGradeCount, exceptList);
        }
    }
}