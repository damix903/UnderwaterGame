using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Manager.Upgrade;

namespace UI
{
    public interface IUpgradeView
    {
        public void ShowUpgrades(List<UpgradeData> upgrades);
        public void HideUpgrade();
        UniTask<UpgradeData> OnUpgradeSelectedAsync(CancellationToken ct);
    }
}