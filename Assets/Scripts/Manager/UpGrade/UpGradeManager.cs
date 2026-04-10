using System.Collections.Generic;
using UnityEngine;

namespace Manager.UpGrade
{
    public class UpGradeManager : MonoBehaviour
    {
        [SerializeField] private UpGradeSetting _upGradeSetting;
        [SerializeField] private List<UpGradeData> _currentUpGradeList = new List<UpGradeData>();

        [ContextMenu("StartUpGrade")]
        public void StartUpGrade()
        {
            var upGrades = _upGradeSetting.GetRandomUpGrade(_currentUpGradeList);
            
            foreach (var upGrade in upGrades)
                Debug.Log(upGrade);
        }
    }
}