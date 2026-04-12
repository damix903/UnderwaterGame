using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manager.Upgrade
{
    [CreateAssetMenu(fileName = "UGD_", menuName = "Data/UpGrade/Data", order = 0)]
    public class UpgradeData : ScriptableObject
    {
        [Header("Visual")]
        [SerializeField] private string upgradeName;
        [SerializeField] private Image image;
        
        [Space]
        [SerializeField] private List<BaseUpgradeEffect> effects; 
        
        public string UpgradeName => upgradeName;
        public Image Image => image;

        public void Apply(RunState state)
        {
            if (effects == null || effects.Count == 0) return;//throw new ArgumentNullException(nameof(upgradeName));
            
            foreach (var e in effects)
                e.Apply(state);
        }
    }
}