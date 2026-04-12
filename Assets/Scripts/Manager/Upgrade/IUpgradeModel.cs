using System;
using System.Collections.Generic;

namespace Manager.Upgrade
{
    public interface IUpgradeModel
    {
        event Action<List<UpgradeData>> OnUpgradeSelection;
        void SelectUpGrade(UpgradeData upgrade);
    }
}