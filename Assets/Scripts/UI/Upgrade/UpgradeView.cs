using System;
using System.Collections.Generic;
using Manager.Upgrade;
using UnityEngine;

namespace UI
{
    public class UpgradeView : MonoBehaviour, IUpgradeView
    {
        [SerializeField] private List<UpgradeElement> upGradeElements = new List<UpgradeElement>();
        
        public event Action<UpgradeData> OnUpgradeSelected;
        
        public void ShowUpgrade(List<UpgradeData> upgrades)
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                if (i >= upGradeElements.Count)
                {
                    Debug.LogWarning("Not enough UpGradeElements to display all upgrades.");
                    break;
                }
                
                var element = upGradeElements[i];
                var data = upgrades[i];
                
                element.RemoveListener();
                element.SetUp(data, (x) => { OnUpgradeSelected?.Invoke(x); });
                element.gameObject.SetActive(true);
            }
        }

        public void HideUpgrade()
        {
            foreach (var element in upGradeElements)
            {
                element.RemoveListener();
                element.gameObject.SetActive(false);
            }
        }
    }
}