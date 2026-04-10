using System;
using System.Collections.Generic;
using ProjectileSystem;
using UnityEngine;

namespace Manager.UpGrade
{
    [CreateAssetMenu(fileName = "UGD_", menuName = "Data/UpGrade/Data", order = 0)]
    public class UpGradeData : ScriptableObject
    {
        [SerializeField] private string upgradeName;
        [SerializeField] private List<BaseUpGradeEffect> effects; 
        
        public string UpgradeName => upgradeName;

        public void Apply(RunTimeState state)
        {
            if (effects == null || effects.Count == 0) throw new ArgumentNullException(nameof(upgradeName));
            
            foreach (var e in effects)
                e.Apply(state);
        }
    }

    public class RunTimeState
    {
        public List<IProjectileModifier> ProjModifiers = new List<IProjectileModifier>();
        
    }
}