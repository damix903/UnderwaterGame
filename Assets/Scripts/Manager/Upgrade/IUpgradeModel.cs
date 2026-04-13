using System;
using System.Collections.Generic;

namespace Manager.Upgrade
{
    public interface IUpgradeModel
    {
        List<UpgradeData> GetUpGradeList();
        void SelectUpGrade(UpgradeData upgrade);
    }
}