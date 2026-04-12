using System;
using System.Collections.Generic;
using Manager.Upgrade;

namespace UI
{
    public interface IUpgradeView
    {
        public event Action<UpgradeData> OnUpgradeSelected;
        public void ShowUpgrade(List<UpgradeData> upgrades);
        public void HideUpgrade();
    }
}