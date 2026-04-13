using System;
using System.Collections.Generic;

namespace Manager.Upgrade
{
    public interface IUpgradeModel
    {
        event Action<List<UpgradeData>> OnUpgradePhaseStarted;
        void SelectUpGrade(UpgradeData upgrade);
    }
}