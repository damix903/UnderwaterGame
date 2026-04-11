using System;
using System.Collections.Generic;
using UnityEngine;

namespace Manager.UpGrade
{
    [CreateAssetMenu(fileName = "UGD_", menuName = "Data/UpGrade/Data", order = 0)]
    public class UpGradeData : ScriptableObject
    {
        [SerializeField] private string upgradeName;
        [SerializeField] private List<BaseUpGradeEffect> effects; 
        
        public string UpgradeName => upgradeName;

        public void Apply(RunState state)
        {
            if (effects == null || effects.Count == 0) return;//throw new ArgumentNullException(nameof(upgradeName));
            
            foreach (var e in effects)
                e.Apply(state);
        }
    }
}