using System;
using System.Collections.Generic;
using Manager.Upgrade;
using VContainer.Unity;

namespace UI
{
    public class UpgradePresenter : IInitializable, IDisposable
    {
        private readonly IUpgradeView _view;
        private readonly IUpgradeModel _model;
        
        public UpgradePresenter(IUpgradeView view, IUpgradeModel model)
        {
            _view = view;
            _model = model;
        }

        public void Initialize()
        {
            _model.OnUpgradeSelection += HandleUpgradeSelection;
            _view.OnUpgradeSelected += HandleUpgradeSelected;
            _view.HideUpgrade();
        }
        
        private void HandleUpgradeSelection(List<UpgradeData> upgrades)
        {
            _view.ShowUpgrade(upgrades);
        }

        private void HandleUpgradeSelected(UpgradeData upgrade)
        {
            _model.SelectUpGrade(upgrade);
            _view.HideUpgrade();
        }
        
        public void Dispose()
        {
            _model.OnUpgradeSelection -= HandleUpgradeSelection;
            _view.OnUpgradeSelected -= HandleUpgradeSelected;
            _view?.HideUpgrade();
        }
    }
}